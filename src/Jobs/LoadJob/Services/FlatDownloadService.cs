using System;
using System.Globalization;
using System.Security.Policy;
using System.Text.Json;
using Interfaces.Models;
using LoadJob.Interfaces;
using LoadJob.Models;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace LoadJob.Services;

public class FlatDownloadService : IFlatDownloadService
{
    private static readonly AsyncRetryPolicy _retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(
        Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 50)
        .Select(s => TimeSpan.FromTicks(Math.Min(s.Ticks, TimeSpan.FromSeconds(25).Ticks))));

    private readonly HttpClient _httpClient;

    public FlatDownloadService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HashSet<FlatDTO>> Load(CityDTO city, CancellationToken cancellationToken)
    {
        var result = new HashSet<FlatDTO>();
        var productsCountPerPage = 22;

        var productsCount = await LoadFlatsCount(city, cancellationToken);
        var pages = (productsCount / productsCountPerPage) + 1;

        var tasks = new List<Task<FlatResponse?>>();

        for (var page = 1; page <= pages; page++)
        {
            var url = GetMyHomeUrl(city, page);

            tasks.Add(_retryPolicy.ExecuteAsync(async () =>
            {
                var resp = await _httpClient.GetAsync(url);
                var str = await resp.Content.ReadAsStringAsync();
                var flatResponse = JsonSerializer.Deserialize<FlatResponse>(str);
                return flatResponse;
            }));

            if (page % 10 == 0)
            {
                await Task.WhenAll(tasks);
            }
        }

        await Task.WhenAll(tasks);

        foreach (var task in tasks)
        {
            var response = task.Result;
            if (response == null)
            {
                continue;
            }

            foreach (var product in response.Data.Products)
            {
                if (product.ProductId == null ||
                    product.Price == null)
                {
                    continue;
                }

                if (!decimal.TryParse(product.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                {
                    continue;
                }

                result.Add(new FlatDTO
                {
                    City = city.Name,
                    Price = price,
                    ProductId = product.ProductId,
                });
            }
        }

        return result;
    }

    private static string GetMyHomeUrl(CityDTO city, int? page = null)
    {
        var result = $@"https://www.myhome.ge/ka/s/qiravdeba-bina-{city.Name}?Keyword={city.NameGe}";
        result += "&AdTypeID=3&PrTypeID=1";
        result += $"&mapC={city.MapC}";
        result += $"&regions={city.Regions}";
        result += $"&fullregions={city.FullRegions}";
        result += $"&districts={city.Districts}";
        result += $"&cities={city.Cities}";
        result += $"&GID={city.GID}";
        result += $"&Ajax=1";

        if (page != null)
        {
            result += $"&Page={page}";
        }

        return result;
    }

    private async Task<int> LoadFlatsCount(CityDTO city, CancellationToken cancellationToken)
    {
        var url = GetMyHomeUrl(city);

        var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetStringAsync(url));

        if (response == null)
        {
            return default;
        }

        var result = JsonSerializer.Deserialize<FlatResponse>(response);

        if (result == null)
        {
            return default;
        }

        int.TryParse(result.Data.Count, out var count);

        return count;
    }
}

using Interfaces;
using Interfaces.Models;
using Repository.Bot;

namespace Services;

public class FilterService : IFilterService
{
    private readonly FilterRepository _filterRepository;
    private readonly CityRepository _cityRepository;

    public FilterService(
        FilterRepository filterRepository,
        CityRepository cityRepository)
    {
        _filterRepository = filterRepository;
        _cityRepository = cityRepository;
    }

    public async Task<FilterDTO?> GetByUserName(
        string username,
        CancellationToken cancellationToken)
    {
        var filter = await _filterRepository.GetByUserName(username, cancellationToken);

        if (filter == null)
        {
            return null;
        }

        return new FilterDTO
        {
            UpPrice = filter.UpPrice,
            DownPrice = filter.DownPrice,
            Id = filter.Id,
            CityName = filter.City.Name,
        };
    }

    public async Task UpdateUserCity(
        string username,
        string cityName,
        CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByName(cityName, cancellationToken);

        if (city == null)
        {
            return;
        }

        var filter = await _filterRepository.GetByUserName(username, cancellationToken);

        if (filter == null)
        {
            return;
        }

        filter.City = city;

        _filterRepository.Update(filter);
        await _filterRepository.SaveAsync();
    }

    public async Task UpdateUserPrice(
        string username,
        decimal downPrice,
        decimal upPrice,
        CancellationToken cancellationToken)
    {
        var filter = await _filterRepository.GetByUserName(username, cancellationToken);

        if (filter == null)
        {
            return;
        }

        filter.DownPrice = downPrice;
        filter.UpPrice = upPrice;

        _filterRepository.Update(filter);
        await _filterRepository.SaveAsync();
    }
}

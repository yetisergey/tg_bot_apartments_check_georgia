using BotApp.Interfaces;
using BotApp.Services.Base;
using BotApp.Services.Handlers;
using Configuration;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Bot;
using Repository.Flats;
using Services;
using Storage.Bot;
using Storage.Flats;
using Telegram.Bot;

namespace BotApp.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<BotContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Bot"),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

        services.AddScoped<IEnumerable<FlatContext>>((sp) =>
        {
            return new List<FlatContext>
            {
                new FlatContext(configuration.GetConnectionString("FlatShard0")),
                new FlatContext(configuration.GetConnectionString("FlatShard1")),
                new FlatContext(configuration.GetConnectionString("FlatShard2")),
            };
        });

        services.Scan(scan => scan
            .AddTypes(
                typeof(CityRepository),
                typeof(FilterRepository),
                typeof(UserRepository),
                typeof(FlatRepository))
            .AsSelf()
            .WithTransientLifetime());

        var telegramConfiguration = configuration.GetRequiredSection(nameof(TelegramConfiguration)).Get<TelegramConfiguration>();
        services.AddSingleton(telegramConfiguration);

        services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    var config = sp.GetRequiredService<TelegramConfiguration>();
                    return new TelegramBotClient(config.TelegramToken, httpClient);
                });

        services.AddTransient<IBotHandler, BotHandler>();
        services.AddTransient<ReceiverService>();
        services.AddTransient<ICityService, CityService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IFilterService, FilterService>();
        services.AddHostedService<PollingService>();
    }
}

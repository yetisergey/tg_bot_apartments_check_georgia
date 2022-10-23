using Configuration;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using NotifyJob.Interfaces;
using NotifyJob.Services;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Extensions;
using RabbitMQ.Interfaces;
using Repository.Bot;
using Repository.Flats;
using Services;
using Storage.Bot;
using Storage.Flats;
using Telegram.Bot;

namespace NotifyJob.Extensions;

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

        services.AddSingleton<NotifyMetricService>();

        services.AddRabbitMQClient(configuration);

        services.AddDbContext<BotContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Bot")));

        services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    var config = sp.GetRequiredService<TelegramConfiguration>();
                    return new TelegramBotClient(config.TelegramToken, httpClient);
                });

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICityService, CityService>();
        services.AddTransient<INotificationService, NotificationService>();

        services.AddTransient<IRabbitMQConsumerBase, FlatConsumer>();
    }
}

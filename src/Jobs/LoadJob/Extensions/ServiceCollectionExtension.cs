using Configuration;
using Hangfire;
using Interfaces;
using LoadJob.Interfaces;
using LoadJob.Services;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Extensions;
using RabbitMQ.Interfaces;
using RabbitMQ.Services;
using Repository.Bot;
using Repository.Flats;
using Services;
using Storage.Bot;
using Storage.Flats;

namespace LoadJob.Extensions;

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
            return new List<FlatContext> {
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

        services.AddRabbitMQClient(configuration);

        services.AddDbContext<BotContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Bot")));

        services.AddHttpClient<IFlatDownloadService, FlatDownloadService>();
        services.AddTransient<IRabbitMQProducer, RabbitMQProducer>();
        services.AddTransient<ICityService, CityService>();
        services.AddTransient<IFlatService, FlatService>();
        services.AddTransient<IFlatProducer, FlatProducer>();
        services.AddTransient<ISyncService, SyncService>();
        services.AddTransient<IJobService, JobService>();

        GlobalConfiguration.Configuration.UseInMemoryStorage();
        services.AddHangfire(configuration => configuration
            .UseInMemoryStorage());
        services.AddHangfireServer();
    }
}

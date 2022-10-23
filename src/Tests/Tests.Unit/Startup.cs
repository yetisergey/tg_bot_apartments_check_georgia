using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Extensions;

namespace Tests.Unit;
public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder) =>
        hostBuilder
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { "RabbitMQConfiguration:HostName", "127.0.0.1" },
                });
            });

    public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddRabbitMQClient(context.Configuration);
    }
}

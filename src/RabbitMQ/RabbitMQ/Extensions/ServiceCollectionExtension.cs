using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Configuration;
using RabbitMQ.Interfaces;
using RabbitMQ.Services;

namespace RabbitMQ.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddRabbitMQClient(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration
            .GetRequiredSection(nameof(RabbitMQConfiguration))
            .Get<RabbitMQConfiguration>();

        services.AddSingleton(config);

        services.AddTransient((serviceProvider) =>
        {
            var config = serviceProvider.GetRequiredService<RabbitMQConfiguration>();
            var factory = new ConnectionFactory()
            {
                HostName = config.HostName,
            };
            var connection = factory.CreateConnection();
            return connection.CreateModel();
        });

        services.AddTransient<IRabbitMQProducer, RabbitMQProducer>();
        services.AddHostedService<RabbitMQListener>();
    }
}

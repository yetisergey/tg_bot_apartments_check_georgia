using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Interfaces;

namespace RabbitMQ.Services;

public class RabbitMQListener : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<RabbitMQListener> _logger;

    public RabbitMQListener(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<RabbitMQListener> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var assemblyFullName = Assembly.GetEntryAssembly().GetName().Name;
        var scope = _serviceScopeFactory.CreateScope();
        var channel = scope.ServiceProvider.GetRequiredService<IModel>();
        var consumers = scope.ServiceProvider.GetRequiredService<IEnumerable<IRabbitMQConsumerBase>>();

        foreach (var consumer in consumers)
        {
            if (consumer == null)
            {
                continue;
            }

            var eventType = consumer!.GetType()
                .GetInterfaces()
                .Where(x => x.IsGenericType)
                .Select(x => x.GetGenericArguments().First())
                .Single();

            var metadata = ContractMetadataProvider.GetMetadata(eventType);

            if (metadata == null)
            {
                continue;
            }

            channel.ExchangeDeclare($"{metadata.Queue}.exchange", ExchangeType.Topic);
            channel.QueueDeclare($"{metadata.Queue}.queue.log", false, false, false, null);
            channel.QueueBind($"{metadata.Queue}.queue.log", $"{metadata.Queue}.exchange", $"{metadata.Queue}.queue.*", null);
            channel.BasicQos(0, 1, false);

            _logger.LogInformation($"Start received messages from queue: {metadata.Queue}");

            var eventingBasicConsumer = new EventingBasicConsumer(channel);
            var activitySource = new ActivitySource(assemblyFullName);

            eventingBasicConsumer.Received += async (ch, ea) =>
            {
                try
                {
                    using (activitySource.StartActivity(eventType.Name))
                    {
                        var body = ea.Body.ToArray();
                        var str = Encoding.UTF8.GetString(body);
                        var obj = JsonConvert.DeserializeObject(str, eventType);
                        var process = consumer.GetType().GetMethods().First();
                        _logger.LogInformation($"Start processing message: {JsonConvert.SerializeObject(obj)}");
                        var parameters = new[] { obj };
                        var task = process!.Invoke(consumer, parameters: parameters) as Task;
                        if (task != null)
                        {
                            await task;
                        }

                        channel.BasicAck(ea.DeliveryTag, false);
                        _logger.LogInformation($"Processing finished");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
            };
            channel.BasicConsume($"{metadata.Queue}.queue.log", false, eventingBasicConsumer);
        }

        return Task.CompletedTask;
    }
}

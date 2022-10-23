using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Interfaces;

namespace RabbitMQ.Services;

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQProducer> _logger;

    public RabbitMQProducer(
        IModel channel,
        ILogger<RabbitMQProducer> logger)
    {
        _channel = channel;
        _logger = logger;
    }

    public void ProduceMessage<T>(T message)
        where T : class
    {
        _logger.LogInformation($"Begin {nameof(ProduceMessage)}");

        var metadata = ContractMetadataProvider.GetMetadata(typeof(T));

        if (metadata == null)
        {
            return;
        }

        _channel.ExchangeDeclare($"{metadata.Queue}.exchange", ExchangeType.Topic);
        _channel.QueueDeclare($"{metadata.Queue}.queue.log", false, false, false, null);
        _channel.QueueBind($"{metadata.Queue}.queue.log", $"{metadata.Queue}.exchange", $"{metadata.Queue}.queue.*", null);

        var obj = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(obj);

        _channel.BasicPublish(
            exchange: $"{metadata.Queue}.exchange",
            routingKey: $"{metadata.Queue}.queue.*",
            basicProperties: null,
            body: body);

        _logger.LogInformation($"End {nameof(ProduceMessage)}");
    }
}

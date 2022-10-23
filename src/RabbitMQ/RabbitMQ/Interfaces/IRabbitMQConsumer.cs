namespace RabbitMQ.Interfaces;

public interface IRabbitMQConsumer<T> : IRabbitMQConsumerBase
        where T : class
{
    Task Process(T message);
}

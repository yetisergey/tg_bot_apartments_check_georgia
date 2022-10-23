namespace RabbitMQ.Interfaces;

public interface IRabbitMQProducer
{
    void ProduceMessage<T>(T message)
        where T : class;
}

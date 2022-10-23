using RabbitMQ.Interfaces;
using Tests.Unit.Rabbit.Contracts;

namespace Tests.Unit.Rabbit;

public class ProduceTest
{
    private readonly IRabbitMQProducer _rabbitMQProducer;

    public ProduceTest(IRabbitMQProducer rabbitMQProducer)
    {
        _rabbitMQProducer = rabbitMQProducer;
    }

    [Fact]
    public void Produce_TestContract_Test()
    {
        _rabbitMQProducer.ProduceMessage(new TestContract());
    }
}

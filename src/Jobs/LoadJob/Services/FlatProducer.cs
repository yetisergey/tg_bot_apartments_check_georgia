using Interfaces.Models;
using LoadJob.Interfaces;
using RabbitMQ.Contracts;
using RabbitMQ.Interfaces;

namespace LoadJob.Services;

public class FlatProducer : IFlatProducer
{
    private readonly IRabbitMQProducer _rabbitMQProducer;

    public FlatProducer(IRabbitMQProducer rabbitMQProducer)
    {
        _rabbitMQProducer = rabbitMQProducer;
    }

    public void Produce(CityDTO city, HashSet<FlatDTO> flats, CancellationToken cancellationToken)
    {
        _rabbitMQProducer.ProduceMessage(new NewFlatContract { City = city, Flats = flats });
    }
}

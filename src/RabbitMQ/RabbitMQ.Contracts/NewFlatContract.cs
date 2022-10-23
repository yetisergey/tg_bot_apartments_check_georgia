using Interfaces.Models;
using RabbitMQ.Attributes;

namespace RabbitMQ.Contracts;

[RabbitMQContract("flats")]
public class NewFlatContract
{
    public CityDTO City { get; set; } = new CityDTO();

    public HashSet<FlatDTO> Flats { get; set; } = new HashSet<FlatDTO>();
}

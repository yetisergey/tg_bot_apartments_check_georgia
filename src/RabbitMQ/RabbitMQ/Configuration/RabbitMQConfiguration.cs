namespace RabbitMQ.Configuration;

public record RabbitMQConfiguration
{
    public string HostName { get; set; } = null!;
}

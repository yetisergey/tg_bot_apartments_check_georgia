namespace RabbitMQ.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class RabbitMQContract : Attribute
{
    public RabbitMQContract(string queue)
    {
        Queue = queue;
    }

    public string Queue { get; }
}

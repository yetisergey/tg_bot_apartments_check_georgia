using System.Reflection;
using RabbitMQ.Attributes;

namespace RabbitMQ.Services;

public static class ContractMetadataProvider
{
    public static RabbitMQContract? GetMetadata(Type t) => t.GetCustomAttribute<RabbitMQContract>();
}

using System.Diagnostics.Metrics;
using System.Reflection;

namespace NotifyJob.Services;

public class NotifyMetricService
{
    private readonly Meter _meter;

    public Counter<int> NewFlatCounter { get; set; }

    public NotifyMetricService()
    {
        var assemblyFullName = Assembly.GetEntryAssembly().GetName().Name;
        _meter = new Meter(assemblyFullName);
        NewFlatCounter = _meter.CreateCounter<int>(nameof(NewFlatCounter));
    }
}

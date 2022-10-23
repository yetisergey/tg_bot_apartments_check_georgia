using Interfaces;
using LoadJob.Interfaces;

namespace LoadJob.Services;

public class JobService : IJobService
{
    private readonly ILogger<JobService> _logger;
    private readonly ICityService _cityService;
    private readonly IFlatProducer _flatProducer;
    private readonly ISyncService _syncService;

    public JobService(
        ILogger<JobService> logger,
        ICityService cityService,
        IFlatProducer flatProducer,
        ISyncService syncService)
    {
        _logger = logger;
        _cityService = cityService;
        _flatProducer = flatProducer;
        _syncService = syncService;
    }

    public async Task ExecuteAsync()
    {
        var cities = await _cityService.Get();
        foreach (var city in cities)
        {
            var added = await _syncService.SyncFlatsAsync(city);
            _flatProducer.Produce(city, added);
        }
    }
}

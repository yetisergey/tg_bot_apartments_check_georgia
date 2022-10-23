using Interfaces;
using Interfaces.Models;
using LoadJob.Interfaces;

namespace LoadJob.Services;

public class SyncService : ISyncService
{
    private readonly IFlatService _flatService;
    private readonly IFlatDownloadService _flatDownloadService;
    private readonly ILogger<SyncService> _logger;

    public SyncService(
        IFlatService flatService,
        IFlatDownloadService flatDownloadService,
        ILogger<SyncService> logger)
    {
        _flatService = flatService;
        _flatDownloadService = flatDownloadService;
        _logger = logger;
    }

    public async Task<HashSet<FlatDTO>> SyncFlatsAsync(CityDTO cityDTO)
    {
        var oldFlats = await _flatService.GetByCity(cityDTO);
        var actualFlats = await _flatDownloadService.Load(cityDTO);

        var diff = oldFlats.Except(actualFlats).Concat(actualFlats.Except(oldFlats));
        var added = diff.Intersect(actualFlats).ToHashSet();

        if (added.Any())
        {
            await _flatService.SyncFlats(cityDTO, actualFlats);
        }

        return added;
    }
}

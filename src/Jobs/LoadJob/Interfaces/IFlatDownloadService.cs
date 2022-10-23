using Interfaces.Models;

namespace LoadJob.Interfaces;

public interface IFlatDownloadService
{
    Task<HashSet<FlatDTO>> Load(CityDTO city, CancellationToken cancellationToken = default);
}

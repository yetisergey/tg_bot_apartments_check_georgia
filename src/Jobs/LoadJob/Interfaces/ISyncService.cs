using Interfaces.Models;

namespace LoadJob.Interfaces;

public interface ISyncService
{
    Task<HashSet<FlatDTO>> SyncFlatsAsync(CityDTO cityDTO);
}

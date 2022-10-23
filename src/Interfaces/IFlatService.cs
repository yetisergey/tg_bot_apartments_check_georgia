using Interfaces.Models;

namespace Interfaces;

public interface IFlatService
{
    Task<HashSet<FlatDTO>> GetByCity(CityDTO city, CancellationToken cancellationToken = default);

    Task SyncFlats(CityDTO city, HashSet<FlatDTO> products, CancellationToken cancellationToken = default);
}

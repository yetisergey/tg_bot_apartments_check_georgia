using Interfaces.Models;

namespace Interfaces;

public interface ICityService
{
    Task<List<CityDTO>> Get(CancellationToken cancellationToken = default);

    Task<CityDTO?> GetDefaultUserCity(CancellationToken cancellationToken = default);
}

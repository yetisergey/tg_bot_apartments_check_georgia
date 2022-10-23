using Interfaces.Models;

namespace Interfaces;

public interface IFilterService
{
    Task<FilterDTO?> GetByUserName(string username, CancellationToken cancellationToken);

    Task UpdateUserCity(string username, string city, CancellationToken cancellationToken);

    Task UpdateUserPrice(string username, decimal downPrice, decimal upPrice, CancellationToken cancellationToken);
}

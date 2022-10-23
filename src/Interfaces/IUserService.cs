using Interfaces.Models;

namespace Interfaces;

public interface IUserService
{
    Task<bool> Any(string userName, CancellationToken cancellationToken);

    Task<List<UserDTO>> GetByFilter(string city, decimal price);

    Task Add(string userName, long chatId, CancellationToken cancellationToken);
}

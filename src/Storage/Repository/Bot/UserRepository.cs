using Microsoft.EntityFrameworkCore;
using Storage.Bot;
using Storage.Bot.Models;

namespace Repository.Bot;

public class UserRepository : GenericRepository<User>
{
    private readonly BotContext _context;

    public UserRepository(BotContext context)
        : base(context)
    {
        _context = context;
    }

    public Task<List<User>> GetByFilter(string city, decimal price)
    {
        return _context.Filters
             .Include(x => x.User)
             .Include(x => x.City)
             .Where(x => x.City.Name == city)
             .Where(x => x.DownPrice <= price && price <= x.UpPrice)
             .Select(x => x.User)
             .ToListAsync();
    }

    public Task<bool> AnyUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return AnyAsync(x => x.Nickname == userName, cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using Storage.Bot;
using Storage.Bot.Models;

namespace Repository.Bot;

public class CityRepository : GenericRepository<City>
{
    private readonly BotContext _context;

    public CityRepository(BotContext context)
        : base(context)
    {
        _context = context;
    }

    public Task<City?> GetByName(string cityName, CancellationToken cancellationToken)
    {
        return _context.Cities.FirstOrDefaultAsync(x => x.Name == cityName, cancellationToken);
    }

    public Task<City?> GetDefault(CancellationToken cancellationToken)
    {
        return _context.Cities.FirstOrDefaultAsync(x => x.IsDefault, cancellationToken);
    }
}

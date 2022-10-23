using Microsoft.EntityFrameworkCore;
using Storage.Bot;
using Storage.Bot.Models;

namespace Repository.Bot;

public class FilterRepository : GenericRepository<Filter>
{
    private readonly BotContext _context;

    public FilterRepository(BotContext context)
        : base(context)
    {
        _context = context;
    }

    public Task<Filter?> GetByUserName(string username, CancellationToken cancellationToken)
    {
        return _context.Filters
            .Include(x => x.User)
            .Include(x => x.City)
            .Where(x => x.User.Nickname == username)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

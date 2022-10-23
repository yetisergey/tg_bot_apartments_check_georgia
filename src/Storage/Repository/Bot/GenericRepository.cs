using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Storage.Bot;

namespace Repository.Bot;

public abstract class GenericRepository<T>
    where T : class
{
    private readonly BotContext _context;

    public GenericRepository(BotContext context)
    {
        _context = context;
    }

    public Task<T[]> GetAll(CancellationToken cancellationToken)
    {
        return _context.Set<T>().ToArrayAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken) =>
        _context.Set<T>().AnyAsync(expression, cancellationToken);

    public EntityEntry<T> Add(T t) => _context.Set<T>().Add(t);

    public void Update(T entity)
            => _context.Update(entity);

    public Task SaveAsync() => _context.SaveChangesAsync();
}

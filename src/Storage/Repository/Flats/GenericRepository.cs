using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Storage.Flats;
using Storage.Flats.Models;
using Z.EntityFramework.Plus;

namespace Repository.Flats;

public class GenericRepository<T>
    where T : class, ISharded
{
    private readonly FlatContext[] _contexts;

    public GenericRepository(IEnumerable<FlatContext> contexts)
    {
        _contexts = contexts.ToArray();
    }

    public async Task<ICollection<T>> Get(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        var queries = _contexts.Select(x => x.Set<T>().Where(expression).ToListAsync(cancellationToken));
        await Task.WhenAll(queries);
        return queries.SelectMany(x => x.Result).ToList();
    }

    public Task AddRange(T[] items, CancellationToken cancellationToken)
    {
        foreach (var item in items)
        {
            var ctx = GetContext(item.GetShardKey());
            ctx.Set<T>().Add(item);
        }

        return Task.WhenAll(_contexts.Select(x => x.SaveChangesAsync(cancellationToken)));
    }

    public Task Delete(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        const int batchSize = 200;
        var queries = _contexts.Select(x => x.Set<T>().Where(expression)
            .DeleteAsync(x => x.BatchSize = batchSize, cancellationToken));
        return Task.WhenAll(queries);
    }

    private FlatContext GetContext(string shardKey)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(shardKey));
        var x = BitConverter.ToUInt16(hash, 0) % _contexts.Length;
        return _contexts[x];
    }
}

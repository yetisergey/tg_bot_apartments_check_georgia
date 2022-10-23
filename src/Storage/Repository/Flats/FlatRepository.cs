using Storage.Flats;
using Storage.Flats.Models;

namespace Repository.Flats;

public class FlatRepository : GenericRepository<Flat>
{
    public FlatRepository(IEnumerable<FlatContext> contexts)
        : base(contexts)
    {
    }

    public Task<ICollection<Flat>> GetByCity(string city, CancellationToken cancellationToken)
    {
        return Get(x => x.City == city, cancellationToken);
    }

    public async Task AddByCity(Flat[] flats, CancellationToken cancellationToken)
    {
        const int batchSize = 500;
        foreach (var flatChunk in flats.Chunk(batchSize))
        {
            await AddRange(flatChunk, cancellationToken);
        }
    }

    public Task DeleteByCity(string city, CancellationToken cancellationToken)
    {
        return Delete(x => x.City == city, cancellationToken);
    }
}

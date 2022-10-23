using Interfaces;
using Interfaces.Models;
using Repository.Flats;
using Storage.Flats.Models;

namespace Services;

public class FlatService : IFlatService
{
    private readonly FlatRepository _flatRepository;

    public FlatService(FlatRepository flatRepository)
    {
        _flatRepository = flatRepository;
    }

    public async Task<HashSet<FlatDTO>> GetByCity(CityDTO city, CancellationToken cancellationToken)
    {
        var flats = await _flatRepository.GetByCity(city.Name, cancellationToken);
        return flats.Select(x => new FlatDTO
        {
            City = x.City,
            ProductId = x.ProductId,
            Price = x.Price,
        }).ToHashSet();
    }

    public async Task SyncFlats(CityDTO city, HashSet<FlatDTO> products, CancellationToken cancellationToken)
    {
        await _flatRepository.DeleteByCity(city.Name, cancellationToken);
        var flats = products.Select(x => new Flat
        {
            City = city.Name,
            Price = x.Price,
            ProductId = x.ProductId,
        }).ToArray();
        await _flatRepository.AddByCity(flats, cancellationToken);
    }
}

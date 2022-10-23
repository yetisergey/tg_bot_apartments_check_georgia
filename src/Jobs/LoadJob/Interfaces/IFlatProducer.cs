using Interfaces.Models;

namespace LoadJob.Interfaces;

public interface IFlatProducer
{
    void Produce(CityDTO city, HashSet<FlatDTO> added, CancellationToken cancellationToken = default);
}

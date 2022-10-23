using Interfaces;
using Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Bot;

namespace Services;

public class CityService : ICityService
{
    private readonly CityRepository _cityRepository;

    public CityService(CityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<List<CityDTO>> Get(CancellationToken cancellationToken)
    {
        var cities = await _cityRepository.GetAll(cancellationToken);
        return cities
            .Select(x => new CityDTO
            {
                Id = x.Id,
                Name = x.Name,
                NameGe = x.NameGe,
                Regions = x.Regions,
                MapC = x.MapC,
                FullRegions = x.FullRegions,
                Cities = x.Cities,
                GID = x.GID,
                Districts = x.Districts,
                IsDefault = x.IsDefault,
            })
            .ToList();
    }

    public async Task<CityDTO?> GetDefaultUserCity(CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetDefault(cancellationToken);

        if (city == null)
        {
            return null;
        }

        return new CityDTO
            {
                Id = city.Id,
                Name = city.Name,
                NameGe = city.NameGe,
                MapC = city.MapC,
            Regions = city.Regions,
                FullRegions = city.FullRegions,
                Cities = city.Cities,
                GID = city.GID,
                Districts = city.Districts,
                IsDefault = city.IsDefault,
            };
    }
}

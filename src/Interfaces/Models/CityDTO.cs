namespace Interfaces.Models;

public class CityDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string NameGe { get; set; } = null!;

    public bool IsDefault { get; set; }

    public string GID { get; set; } = null!;

    public string Cities { get; set; } = null!;

    public string Districts { get; set; } = null!;

    public string FullRegions { get; set; } = null!;

    public string Regions { get; set; } = null!;

    public string MapC { get; set; } = null!;
}

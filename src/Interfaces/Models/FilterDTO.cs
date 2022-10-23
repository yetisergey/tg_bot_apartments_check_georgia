namespace Interfaces.Models;

public class FilterDTO
{
    public int Id { get; set; }

    public decimal DownPrice { get; set; }

    public decimal UpPrice { get; set; }

    public string CityName { get; set; } = null!;
}

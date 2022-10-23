namespace Storage.Flats.Models;

public class Flat : ISharded
{
    public int Id { get; set; }

    public string ProductId { get; set; } = null!;

    public decimal Price { get; set; }

    public string City { get; set; } = null!;

    public string GetShardKey()
    {
        return ProductId;
    }
}

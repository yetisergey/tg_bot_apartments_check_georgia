namespace Interfaces.Models;

public record FlatDTO
{
    public string ProductId { get; set; } = null!;

    public string City { get; set; } = null!;

    public decimal Price { get; set; }
}

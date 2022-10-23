using System.Text.Json.Serialization;

namespace LoadJob.Models;

public record ProductResponse
{
    [JsonPropertyName("product_id")]
    public string? ProductId { get; set; }

    [JsonPropertyName("price")]
    public string? Price { get; set; }
}

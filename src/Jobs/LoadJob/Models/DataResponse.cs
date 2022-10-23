using System.Text.Json.Serialization;

namespace LoadJob.Models;

public record DataResponse
{
    [JsonPropertyName("Cnt")]
    public string? Count { get; set; }

    [JsonPropertyName("Prs")]
    public IReadOnlyCollection<ProductResponse> Products { get; set; } = new List<ProductResponse>();
}

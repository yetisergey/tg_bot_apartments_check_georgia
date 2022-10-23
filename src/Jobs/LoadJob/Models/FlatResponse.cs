namespace LoadJob.Models;

public record FlatResponse
{
    public DataResponse Data { get; set; } = new DataResponse();
}

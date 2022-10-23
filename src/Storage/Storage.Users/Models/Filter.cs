namespace Storage.Bot.Models;

public class Filter
{
    public int Id { get; set; }

    public decimal UpPrice { get; set; }

    public decimal DownPrice { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public int CityId { get; set; }

    public virtual City City { get; set; } = null!;
}

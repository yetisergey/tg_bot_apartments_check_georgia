namespace Interfaces.Models;

public class UserDTO
{
    public int Id { get; set; }

    public string Nickname { get; set; } = null!;

    public long ChatId { get; set; }
}

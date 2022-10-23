using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Storage.Bot.Models;

[Index(nameof(Nickname), Name = nameof(Nickname), IsUnique = true)]
public class User
{
    public int Id { get; set; }

    [Required]
    public string Nickname { get; set; } = null!;

    [Required]
    public long ChatId { get; set; }
}

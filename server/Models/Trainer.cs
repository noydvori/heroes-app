using System.ComponentModel.DataAnnotations;

namespace HeroesApi.Models;

public class Trainer
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public byte[] PasswordHash { get; set; } = null!;

    [Required]
    public byte[] PasswordSalt { get; set; } = null!;

    public ICollection<Hero> Heroes { get; set; } = new List<Hero>();
}

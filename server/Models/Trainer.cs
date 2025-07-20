using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesApi.Models;

public class Trainer
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    public byte[] PasswordHash { get; set; } = null!;

    [Required]
    public byte[] PasswordSalt { get; set; } = null!;

    // Navigation Property - One trainer has many heroes
    public ICollection<Hero> Heroes { get; set; } = new List<Hero>();
}

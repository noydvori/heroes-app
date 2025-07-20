using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesApi.Models;
public enum HeroAbility { Attacker, Defender }

public class Hero
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    public HeroAbility Ability { get; set; }

    [Required]
    [MaxLength(30)]
    public string SuitColors { get; set; } = null!;

    [Required]
    public decimal StartingPower { get; set; }

    [Required]
    public decimal CurrentPower { get; set; }

    public DateTime StartTrainingDate { get; set; } = DateTime.UtcNow;

    public int TrainingsToday { get; set; }

    public DateTime? LastTrainingDate { get; set; }

    // Foreign Key
    [Required]
    public Guid TrainerId { get; set; }

    // Navigation Property
    [ForeignKey(nameof(TrainerId))]
    public Trainer Trainer { get; set; } = null!;
}

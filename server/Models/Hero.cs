using System.ComponentModel.DataAnnotations;

namespace HeroesApi.Models;

public class Hero
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(20)]
    public string Ability { get; set; } = null!; // attacker / defender

    public DateTime StartTrainingDate { get; set; }

    [Required]
    public string SuitColors { get; set; } = null!;

    public decimal StartingPower { get; set; }
    public decimal CurrentPower { get; set; }

    public int TrainingsToday { get; set; }
    public DateTime? LastTrainingDate { get; set; }

    // Foreign Key
    public Guid TrainerId { get; set; }

    // Navigation Property
    public Trainer Trainer { get; set; } = null!;
}

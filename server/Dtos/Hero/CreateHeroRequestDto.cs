using System.ComponentModel.DataAnnotations;

namespace HeroesApi.Dtos.Hero;

public class CreateHeroRequestDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [Required, RegularExpression("^(attacker|defender)$")]
    public string Ability { get; set; } = null!;

    public string SuitColors { get; set; } = null!;

    [Required]
    public decimal StartingPower { get; set; }
}

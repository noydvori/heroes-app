using System.ComponentModel.DataAnnotations;

public class CreateHeroRequestDto
{
    [Required, StringLength(50)]
    public string Name { get; set; } = null!;
    
    [Required, RegularExpression("^(attacker|defender)$")]
    public string Ability { get; set; } = null!;

    public string SuitColors { get; set; } = null!;

    
    [Required, Range(1, 1000)]
    public decimal StartingPower { get; set; }
}

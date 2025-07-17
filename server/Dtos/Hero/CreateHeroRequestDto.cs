public class CreateHeroRequestDto
{
    public string Name { get; set; } = null!;
    public string Ability { get; set; } = null!; // attacker / defender
    public string SuitColors { get; set; } = null!;
    public decimal StartingPower { get; set; }
}
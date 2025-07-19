namespace HeroesApi.Dtos.Hero;

public class HeroResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Ability { get; set; } = null!;
    public DateTime StartTrainingDate { get; set; }
    public string SuitColors { get; set; } = null!;
    public decimal StartingPower { get; set; }
    public decimal CurrentPower { get; set; }
    public Guid TrainerId { get; set; }
}
public interface IHeroService
{
    Task<List<HeroResponseDto>> GetMyHeroesAsync();
    Task<HeroResponseDto?> CreateHeroAsync(CreateHeroRequestDto dto);
    Task<HeroTrainingResultDto> TrainHeroAsync(Guid heroId);
}

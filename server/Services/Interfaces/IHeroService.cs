using HeroesApi.Dtos.Hero;

namespace HeroesApi.Services.Interfaces;

public interface IHeroService
{
    Task<List<HeroResponseDto>> GetMyHeroesAsync();
    Task<List<HeroResponseDto>> GetAllHeroesAsync();
    Task<HeroResponseDto?> CreateHeroAsync(CreateHeroRequestDto dto);
    Task<HeroTrainingResultDto> TrainHeroAsync(Guid heroId);
    Task<HeroResponseDto?> UpdateHeroAsync(Guid id, HeroUpdateRequest dto);
    Task<bool> DeleteHeroAsync(Guid id);
}

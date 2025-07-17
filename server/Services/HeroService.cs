using HeroesApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class HeroService : IHeroService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HeroService> _logger;

    public HeroService(AppDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<HeroService> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    private Guid GetTrainerId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Trainer ID not found in token.");
            throw new UnauthorizedAccessException("Trainer ID not found in token.");
        }

        return Guid.Parse(userId);
    }

    public async Task<List<HeroResponseDto>> GetMyHeroesAsync()
    {
        var trainerId = GetTrainerId();

        var heroes = await _context.Heroes
            .Where(h => h.TrainerId == trainerId)
            .OrderByDescending(h => h.CurrentPower)
            .ToListAsync();

        _logger.LogInformation("Retrieved {Count} heroes for trainer {TrainerId}", heroes.Count, trainerId);

        return heroes.Select(MapToDto).ToList();
    }

    public async Task<HeroResponseDto?> CreateHeroAsync(CreateHeroRequestDto dto)
    {
        var trainerId = GetTrainerId();

        var hero = new Hero
        {
            Name = dto.Name.Trim(),
            Ability = dto.Ability.Trim().ToLower(),
            SuitColors = dto.SuitColors.Trim(),
            StartingPower = dto.StartingPower,
            CurrentPower = dto.StartingPower,
            StartTrainingDate = DateTime.UtcNow,
            TrainerId = trainerId
        };

        _context.Heroes.Add(hero);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Hero {HeroId} created by trainer {TrainerId}", hero.Id, trainerId);

        return MapToDto(hero);
    }

    public async Task<HeroTrainingResultDto> TrainHeroAsync(Guid heroId)
    {
        var trainerId = GetTrainerId();

        var hero = await _context.Heroes
            .FirstOrDefaultAsync(h => h.Id == heroId && h.TrainerId == trainerId);

        if (hero == null)
        {
            _logger.LogWarning("Trainer {TrainerId} attempted to train nonexistent hero {HeroId}", trainerId, heroId);
            return new HeroTrainingResultDto { Success = false, Message = "Hero not found" };
        }

        if (hero.LastTrainingDate?.Date != DateTime.UtcNow.Date)
        {
            hero.TrainingsToday = 0;
            hero.LastTrainingDate = DateTime.UtcNow;
        }

        if (hero.TrainingsToday >= 5)
        {
            _logger.LogInformation("Hero {HeroId} already trained 5 times today", heroId);
            return new HeroTrainingResultDto { Success = false, Message = "Hero has already trained 5 times today" };
        }

        var percent = Random.Shared.NextDouble() * 0.10;
        var potentialIncrease = hero.CurrentPower * (decimal)percent;
        var maxAllowedIncrease = 100 - hero.CurrentPower;

        var actualIncrease = Math.Min(potentialIncrease, maxAllowedIncrease);

        hero.CurrentPower += actualIncrease;
        hero.TrainingsToday++;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Hero training completed. HeroId: {HeroId}, TrainerId: {TrainerId}, PowerGain: {PowerGain}", 
    hero.Id, trainerId, actualIncrease);


        return new HeroTrainingResultDto
{
    Success = true,
    Message = $"Hero trained and gained {actualIncrease:F2} power",
    UpdatedHero = MapToDto(hero)
};
    }

    private static HeroResponseDto MapToDto(Hero h) => new()
    {
        Id = h.Id,
        Name = h.Name,
        Ability = h.Ability,
        SuitColors = h.SuitColors,
        StartTrainingDate = h.StartTrainingDate,
        StartingPower = h.StartingPower,
        CurrentPower = h.CurrentPower
    };
}

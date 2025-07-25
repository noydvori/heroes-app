using System.Security.Claims;
using HeroesApi.Data;
using HeroesApi.Dtos.Hero;
using HeroesApi.Hubs;
using HeroesApi.Models;
using HeroesApi.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HeroesApi.Services;

public class HeroService : IHeroService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HeroService> _logger;
    private readonly IHubContext<HeroHub> _hubContext;


    public HeroService(
    AppDbContext context,
    IHttpContextAccessor httpContextAccessor,
    ILogger<HeroService> logger,
    IHubContext<HeroHub> hubContext)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _hubContext = hubContext;
    }

    private Guid GetTrainerId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("id");
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("TRAINER_AUTH_FAILED: Trainer ID not found in token");
            throw new UnauthorizedAccessException("Trainer ID not found in token.");
        }

        return Guid.Parse(userId);
    }
    public async Task<List<HeroResponseDto>> GetAllHeroesAsync()
    {
        var heroes = await _context.Heroes
            .OrderByDescending(h => h.CurrentPower)
            .ToListAsync();

        _logger.LogInformation("HEROES_GET_ALL_SUCCESS: Retrieved {Count} heroes", heroes.Count);

        return heroes.Select(MapToDto).ToList();
    }

    public async Task<List<HeroResponseDto>> GetMyHeroesAsync()
    {
        var trainerId = GetTrainerId();

        var heroes = await _context.Heroes
            .Where(h => h.TrainerId == trainerId)
            .OrderByDescending(h => h.CurrentPower)
            .ToListAsync();

        _logger.LogInformation("HEROES_GET_MINE_SUCCESS: Retrieved {Count} heroes for trainer {TrainerId}", heroes.Count, trainerId);

        return heroes.Select(MapToDto).ToList();
    }

    public async Task<HeroResponseDto?> CreateHeroAsync(CreateHeroRequestDto dto)
    {
        var trainerId = GetTrainerId();

        if (!Enum.TryParse<HeroAbility>(dto.Ability.Trim(), ignoreCase: true, out var parsedAbility))
        {
            _logger.LogWarning("Invalid ability value received: {Ability}", dto.Ability);
            return null;
        }

        var hero = new Hero
        {
            Name = dto.Name.Trim(),
            Ability = parsedAbility,
            SuitColors = dto.SuitColors.Trim(),
            StartingPower = dto.StartingPower,
            CurrentPower = dto.StartingPower,
            StartTrainingDate = DateTime.UtcNow,
            TrainerId = trainerId
        };

        _context.Heroes.Add(hero);

        await _context.SaveChangesAsync();
        await _hubContext.Clients.All.SendAsync("HeroChanged", MapToDto(hero));
        _logger.LogInformation("HERO_CREATE_SUCCESS: Hero {HeroId} created by trainer {TrainerId}", hero.Id, trainerId);

        return MapToDto(hero);
    }

    public async Task<HeroTrainingResultDto> TrainHeroAsync(Guid heroId)
    {
        var trainerId = GetTrainerId();

        var hero = await _context.Heroes
            .FirstOrDefaultAsync(h => h.Id == heroId && h.TrainerId == trainerId);

        if (hero == null)
        {
            _logger.LogWarning("HERO_TRAIN_FAILED: Hero not found - TrainerId: {TrainerId}, HeroId: {HeroId}", trainerId, heroId);
            return new HeroTrainingResultDto { Success = false, Message = "Hero not found" };
        }

        if (hero.LastTrainingDate?.Date != DateTime.UtcNow.Date)
        {
            hero.TrainingsToday = 0;
            hero.LastTrainingDate = DateTime.UtcNow;
        }

        if (hero.TrainingsToday >= 5)
        {
            _logger.LogInformation("HERO_TRAIN_LIMIT: Daily limit reached - HeroId: {HeroId}", heroId);
            return new HeroTrainingResultDto { Success = false, Message = "Hero has already trained 5 times today" };
        }

        var percent = Random.Shared.NextDouble() * 0.10;
        var increase = hero.CurrentPower * (decimal)percent;

        hero.CurrentPower += increase;
        hero.TrainingsToday++;

        await _context.SaveChangesAsync();
        await _hubContext.Clients.All.SendAsync("HeroListUpdated");

        _logger.LogInformation("HERO_TRAIN_SUCCESS: Training completed - HeroId: {HeroId}, TrainerId: {TrainerId}, PowerGain: {PowerGain:F2}",
    hero.Id, trainerId, increase);


        return new HeroTrainingResultDto
        {
            Success = true,
            Message = $"Hero trained and gained {increase:F2} power",
            UpdatedHero = MapToDto(hero)
        };
    }

    private static HeroResponseDto MapToDto(Hero h) => new()
    {
        Id = h.Id,
        Name = h.Name,
        Ability = h.Ability.ToString(),
        SuitColors = h.SuitColors,
        StartTrainingDate = h.StartTrainingDate,
        StartingPower = h.StartingPower,
        CurrentPower = h.CurrentPower,
        TrainerId = h.TrainerId
    };

    public async Task<HeroResponseDto?> UpdateHeroAsync(Guid id, HeroUpdateRequest request)
    {
        var trainerId = GetTrainerId();

        if (!Enum.TryParse<HeroAbility>(request.Ability.Trim(), ignoreCase: true, out var parsedAbility))
        {
            _logger.LogWarning("Invalid ability value received: {Ability}", request.Ability);
            return null;
        }


        var hero = await _context.Heroes.FindAsync(id);
        if (hero == null) return null;
        if (hero.TrainerId != trainerId) return null;

        hero.Name = request.Name;
        hero.Ability = parsedAbility;
        hero.SuitColors = request.SuitColors;
        hero.CurrentPower = request.StartingPower;

        await _context.SaveChangesAsync();
        return MapToDto(hero);
    }
    
    public async Task<bool> DeleteHeroAsync(Guid id)
    {
        var trainerId = GetTrainerId();

        var hero = await _context.Heroes.FindAsync(id);
        if (hero == null || hero.TrainerId != trainerId)
            return false;

        _context.Heroes.Remove(hero);
        await _context.SaveChangesAsync();
        return true;
    }

}

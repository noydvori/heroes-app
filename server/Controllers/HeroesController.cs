using HeroesApi.Dtos.Hero;
using HeroesApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeroesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HeroesController : ControllerBase
{
    private readonly IHeroService _heroService;

    public HeroesController(IHeroService heroService)
    {
        _heroService = heroService;
    }

    // GET /api/heroes	
    [HttpGet]
    public async Task<ActionResult<List<HeroResponseDto>>> GetMyHeroes()
    {
        return await _heroService.GetMyHeroesAsync();
    }

    // GET /api/heroes/all
    [HttpGet("all")]
    public async Task<ActionResult<List<HeroResponseDto>>> GetAll()
    {
        return await _heroService.GetAllHeroesAsync();
    }

    // POST /api/heroes	
    [HttpPost]
    public async Task<IActionResult> CreateHero([FromBody] CreateHeroRequestDto dto)
    {
        var created = await _heroService.CreateHeroAsync(dto);
        if (created == null)
            return BadRequest("Failed to create hero");

        return Ok(created);
    }

    // POST /api/heroes/train/{id}
    [HttpPost("train/{id}")]
    public async Task<IActionResult> TrainHero([FromRoute] Guid id)
    {
        var result = await _heroService.TrainHeroAsync(id);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result);
    }
}

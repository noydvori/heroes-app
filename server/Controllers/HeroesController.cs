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

    [HttpGet]
    public async Task<ActionResult<List<HeroResponseDto>>> GetMyHeroes()
    {
        return await _heroService.GetMyHeroesAsync();
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<HeroResponseDto>>> GetAll()
    {
        return await _heroService.GetAllHeroesAsync();
    }

    [HttpPost]
    public async Task<IActionResult> CreateHero([FromBody] CreateHeroRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid input");

        var created = await _heroService.CreateHeroAsync(dto);
        if (created == null)
            return BadRequest("Failed to create hero");

        return Ok(created);
    }

    [HttpPost("train/{id}")]
    public async Task<IActionResult> TrainHero(Guid id)
    {
        var result = await _heroService.TrainHeroAsync(id);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result);
    }
}

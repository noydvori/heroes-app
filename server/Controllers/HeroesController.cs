using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HeroController : ControllerBase
{
    private readonly IHeroService _heroService;

    public HeroController(IHeroService heroService)
    {
        _heroService = heroService;
    }

    [HttpGet]
    public async Task<ActionResult<List<HeroResponseDto>>> GetMyHeroes()
    {
        return Ok(await _heroService.GetMyHeroesAsync());
    }

    [HttpPost]
    public async Task<ActionResult<HeroResponseDto>> CreateHero(CreateHeroRequestDto dto)
    {
        var created = await _heroService.CreateHeroAsync(dto);
        if (created == null)
            return BadRequest(new { message = "Failed to create hero" });

        return CreatedAtAction(nameof(GetMyHeroes), new { id = created.Id }, created);
    }

    [HttpPost("train/{id}")]
    public async Task<IActionResult> TrainHero(Guid id)
    {
        var result = await _heroService.TrainHeroAsync(id);
        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(result);
    }
}

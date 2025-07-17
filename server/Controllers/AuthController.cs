using Microsoft.AspNetCore.Mvc;
namespace HeroesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var result = await _authService.RegisterAsync(dto.Email, dto.Password);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(new AuthResponseDto
        {
            Success = true,
            Message = result.Message
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto.Email, dto.Password);

        if (!result.Success)
            return Unauthorized(new { message = result.Message });

        return Ok(new AuthResponseDto
        {
            Success = true,
            Message = result.Message,
            Token = result.Token
        });
    }
}


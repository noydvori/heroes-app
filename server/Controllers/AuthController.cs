using HeroesApi.Dtos.Auth;
using HeroesApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeroesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    // POST /api/auth/register	
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        _logger.LogInformation("CONTROLLER_REGISTER: Attempt for {Email}", dto.Email);
        
        var result = await _authService.RegisterAsync(dto.Email, dto.Password);
        
        if (!result.Success)
        {
            _logger.LogWarning("CONTROLLER_REGISTER_FAILED: {Email} - {Message}", dto.Email, result.Message);
            return BadRequest(result);
        }

        _logger.LogInformation("CONTROLLER_REGISTER_SUCCESS: {Email}", dto.Email);
        return Ok(result);
    }

    // POST /api/auth/login	
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        _logger.LogInformation("CONTROLLER_LOGIN: Attempt for {Email}", dto.Email);
        
        var result = await _authService.LoginAsync(dto.Email, dto.Password);
        
        if (!result.Success)
        {
            _logger.LogWarning("CONTROLLER_LOGIN_FAILED: {Email} - {Message}", dto.Email, result.Message);
            return Unauthorized(result); 
        }

        _logger.LogInformation("CONTROLLER_LOGIN_SUCCESS: {Email} - Token created", dto.Email);
        return Ok(result);
    }
}

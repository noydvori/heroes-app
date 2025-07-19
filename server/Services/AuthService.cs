using System.Security.Cryptography;
using System.Text;
using HeroesApi.Data;
using HeroesApi.Dtos.Auth;
using HeroesApi.Helpers;
using HeroesApi.Models;
using HeroesApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HeroesApi.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, IConfiguration config, ILogger<AuthService> logger)
    {
        _context = context;
        _config = config;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(string email, string password)
    {
        if (await _context.Trainers.AnyAsync(t => t.Email == email))
        {
            _logger.LogWarning("TRAINER_REGISTER_FAILED: Email already exists - {Email}", email);
            return new AuthResponseDto { Success = false, Message = "Email already exists" };
        }

        if (!PasswordValidator.IsValid(password))
        {
            _logger.LogWarning("TRAINER_REGISTER_FAILED: Weak password - {Email}", email);
            return new AuthResponseDto
            {
                Success = false,
                Message = "Password must meet complexity requirements."
            };
        }

        using var hmac = new HMACSHA512();
        var trainer = new Trainer
        {
            Email = email,
            PasswordSalt = hmac.Key,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
        };

        _context.Trainers.Add(trainer);
        await _context.SaveChangesAsync();

        _logger.LogInformation("TRAINER_REGISTER_SUCCESS: New trainer registered - {Email}", email);

        return new AuthResponseDto
        {
            Success = true,
            Message = "Registered successfully"
        };
    }

    public async Task<AuthResponseDto> LoginAsync(string email, string password)
    {
        var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.Email == email);
        if (trainer == null)
        {
            _logger.LogWarning("TRAINER_LOGIN_FAILED: Trainer not found - {Email}", email);
            return new AuthResponseDto { Success = false, Message = "Wrong cradentials" };
        }

        using var hmac = new HMACSHA512(trainer.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        if (!computedHash.SequenceEqual(trainer.PasswordHash))
        {
            _logger.LogWarning("TRAINER_LOGIN_FAILED: Invalid password - {Email}", email);
            return new AuthResponseDto { Success = false, Message = "Wrong cradentials" };
        }

        var token = JwtTokenGenerator.CreateToken(trainer, _config);

        _logger.LogInformation("TRAINER_LOGIN_SUCCESS: Trainer logged in - {Email}", email);

        return new AuthResponseDto
        {
            Success = true,
            Token = token,
            Message = "Login successful"
        };
    }
}

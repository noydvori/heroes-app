using System.Security.Cryptography;
using System.Text;
using HeroesApi.Data;
using Microsoft.EntityFrameworkCore;

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
            _logger.LogWarning("Registration attempt failed: Email {Email} already exists", email);
            return new AuthResponseDto { Success = false, Message = "Email already exists" };
        }

        if (!PasswordValidator.IsValid(password))
        {
            _logger.LogWarning("Registration attempt failed: Weak password provided for {Email}", email);
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

        _logger.LogInformation("New trainer registered successfully with email: {Email}", email);

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
            _logger.LogWarning("Login attempt failed: Trainer not found with email {Email}", email);
            return new AuthResponseDto { Success = false, Message = "Trainer not found" };
        }

        using var hmac = new HMACSHA512(trainer.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        if (!computedHash.SequenceEqual(trainer.PasswordHash))
        {
            _logger.LogWarning("Login attempt failed: Invalid password for email {Email}", email);
            return new AuthResponseDto { Success = false, Message = "Invalid password" };
        }

        var token = JwtTokenGenerator.CreateToken(trainer, _config);

        _logger.LogInformation("Trainer {Email} logged in successfully", email);

        return new AuthResponseDto
        {
            Success = true,
            Token = token,
            Message = "Login successful"
        };
    }
}

using HeroesApi.Dtos.Auth;

namespace HeroesApi.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(string email, string password);
    Task<AuthResponseDto> LoginAsync(string email, string password);
}
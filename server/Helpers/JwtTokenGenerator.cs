using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HeroesApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace HeroesApi.Helpers;

public static class JwtTokenGenerator
{
    public static string CreateToken(Trainer trainer, IConfiguration config)
    {
        var claims = new[]
        {
            new Claim("id", trainer.Id.ToString()),
            new Claim("email", trainer.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtConfig:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: config["JwtConfig:Issuer"],
            audience: config["JwtConfig:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(config["JwtConfig:TokenValidityMins"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

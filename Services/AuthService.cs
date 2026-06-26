using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;


using WarehouseApi.Data;
using WarehouseApi.Models;

namespace WarehouseApi.Services;

public class AuthService(AppDbContext db, IConfiguration config) : IAuthService
{
    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        if (await db.Users.AnyAsync(u => u.Email == dto.Email))
        {
            return null;
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User { Name = dto.Name, Email = dto.Email, PasswordHash = hash };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var token = GenerateToken(user);

        return new AuthResponseDto(token, user.Email, user.Name);

    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user is null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return null;
        }

        var token = GenerateToken(user);
        return new AuthResponseDto(token, user.Email, user.Name);
    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("name", user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
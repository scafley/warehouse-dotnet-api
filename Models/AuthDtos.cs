using System.Runtime.CompilerServices;

namespace WarehouseApi.Models;

public record RegisterDto(string Email, string Password, string Name);

public record LoginDto(string Email, string Password);

public record AuthResponseDto(string Token, string Email, string Name);
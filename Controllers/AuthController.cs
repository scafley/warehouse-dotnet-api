using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Models;
using WarehouseApi.Services;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {

        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest("Email i hasło są wymagane");
        }

        var res = await service.RegisterAsync(dto);
        return res is null ? Conflict("Użytkownik o tym emailu już istnieje") : Ok(res);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var res = await service.LoginAsync(dto);
        return res is null ? Unauthorized("Nieprawidłowe dane logowania") : Ok(res);
    }
}
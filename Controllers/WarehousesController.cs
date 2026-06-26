using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Models;
using WarehouseApi.Services;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WarehousesController(IWarehouseService service) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var userId = GetUserId();
        return Ok(await service.GetMyWarehousesAsync(userId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWarehouseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Nazwa magazynu jest wymagana");
        }

        var userId = GetUserId();
        var created = await service.CreateAsync(userId, dto);

        return CreatedAtAction(nameof(GetMine), null, created);
    }


    private int GetUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return int.Parse(sub!);
    }
}
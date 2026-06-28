using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Models;
using WarehouseApi.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace WarehouseApi.Controllers;



[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController(IProductService service, IStockMovementService movementService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? warehouseId)
    {
        return Ok(await service.GetAllAsync(GetUserId(), warehouseId));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await service.GetByIdAsync(GetUserId(), id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Nazwa jest wymagana");
        }

        var created = await service.CreateAsync(GetUserId(), dto);

        if (created is null) return BadRequest("Brak dostępu do wskazanego magazynu");
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CreateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Nazwa jest wymagana");
        }

        var updated = await service.UpdateAsync(GetUserId(), id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.DeleteAsync(GetUserId(), id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}/movements")]
    public async Task<IActionResult> GetMovements(int id)
    {
        var movements = await movementService.GetMovementsAsync(GetUserId(), id);
        return Ok(movements);
    }

    [HttpPost("{id:int}/movements")]
    public async Task<IActionResult> CreateMovement(int id, CreateMovementDto dto)
    {
        var res = await movementService.CreateMovementAsync(GetUserId(), id, dto);

        if (res.Success)
        {
            return CreatedAtAction(nameof(GetMovements), new { id }, res.Movement);
        }

        switch (res.Error)
        {
            case ("NotFound"):
                return NotFound();
            case ("InsufficientStock"):
                return BadRequest("Niewystarczający stan magazynowy");
            default:
                return BadRequest();
        }

    }

    private int GetUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return int.Parse(sub!);
    }


}
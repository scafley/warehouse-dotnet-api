using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Models;

namespace WarehouseApi.Services;

public class StockMovementService(AppDbContext db) : IStockMovementService
{

    public async Task<MovementResult> CreateMovementAsync(int userId, int productId, CreateMovementDto dto)
    {

        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId && db.UserWarehouses.Any(uw => uw.UserId == userId && uw.WarehouseId == p.WarehouseId));

        if (product is null)
        {
            return new MovementResult(false, "NotFound", null);
        }

        if (dto.Type == MovementType.Out)
        {
            if (dto.Quantity > product.Quantity)
            {
                return new MovementResult(false, "InsufficientStock", null);
            }

            product.Quantity -= dto.Quantity;
        }
        else
        {
            product.Quantity += dto.Quantity;
        }

        var stockMovement = new StockMovement { ProductId = productId, Type = dto.Type, Quantity = dto.Quantity };
        db.StockMovements.Add(stockMovement);
        await db.SaveChangesAsync();


        var res = new MovementResponseDto(stockMovement.Id, stockMovement.ProductId, stockMovement.Type, stockMovement.Quantity, stockMovement.CreatedAt);
        return new MovementResult(true, null, res);
    }

    public async Task<List<MovementResponseDto>> GetMovementsAsync(int userId, int productId)
    {
        return await db.StockMovements
            .Where(m => m.ProductId == productId && db.UserWarehouses.Any(uw => uw.UserId == userId && uw.WarehouseId == m.Product.WarehouseId))
            .OrderBy(m => m.Id)
            .Select(m => new MovementResponseDto(m.Id, m.ProductId, m.Type, m.Quantity, m.CreatedAt))
            .ToListAsync();
    }
}
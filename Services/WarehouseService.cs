using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Models;

namespace WarehouseApi.Services;

public class WarehouseService(AppDbContext db) : IWarehouseService
{


    public async Task<List<WarehouseResponseDto>> GetMyWarehousesAsync(int userId)
    {
        var warehouses = await db.UserWarehouses
                .Where(u => u.UserId == userId)
                .Select(uw => new WarehouseResponseDto(uw.Warehouse.Id, uw.Warehouse.Name))
                .ToListAsync();

        return warehouses;
    }

    public async Task<WarehouseResponseDto> CreateAsync(int userId, CreateWarehouseDto dto)
    {
        var warehouse = new Warehouse { Name = dto.Name };
        db.Warehouses.Add(warehouse);
        await db.SaveChangesAsync();

        var link = new UserWarehouse { UserId = userId, WarehouseId = warehouse.Id };
        db.UserWarehouses.Add(link);
        await db.SaveChangesAsync();

        return new WarehouseResponseDto(warehouse.Id, warehouse.Name);
    }
}
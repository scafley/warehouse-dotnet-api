using WarehouseApi.Models;

namespace WarehouseApi.Services;

public interface IWarehouseService
{
    Task<List<WarehouseResponseDto>> GetMyWarehousesAsync(int userId);

    Task<WarehouseResponseDto> CreateAsync(int userId, CreateWarehouseDto dto);
}
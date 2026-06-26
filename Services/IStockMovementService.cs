using WarehouseApi.Models;
namespace WarehouseApi.Services;

public interface IStockMovementService
{
    Task<MovementResult> CreateMovementAsync(int productId, CreateMovementDto dto);
    Task<List<MovementResponseDto>> GetMovementsAsync(int productId);
}
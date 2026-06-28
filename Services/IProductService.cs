using WarehouseApi.Models;

namespace WarehouseApi.Services;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllAsync(int userId, int? warehouseId);
    Task<ProductResponseDto?> GetByIdAsync(int userId, int id);
    Task<ProductResponseDto?> CreateAsync(int userId, CreateProductDto dto);

    Task<bool> UpdateAsync(int userId, int id, CreateProductDto dto);
    Task<bool> DeleteAsync(int userId, int id);
}
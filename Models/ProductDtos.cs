//using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.Models;

public record CreateProductDto(
 string Name, string Sku, int Quantity, int MinQuantity, int CategoryId
);

public record ProductResponseDto(int Id, string Name, string Sku, int Quantity, int MinQuantity, DateTime CreatedAt, string CategoryName);
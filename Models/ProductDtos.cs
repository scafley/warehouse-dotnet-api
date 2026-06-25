//using System.ComponentModel.DataAnnotations;

namespace WarehouseApi.Models;

public record CreateProductDto(
 string Name, string Sku, int Quantity, int MinQuantity
);

public record ProductResponseDto(int Id, string Name, string Sku, int Quantity, int MinQuantity, DateTime CreatedAt);
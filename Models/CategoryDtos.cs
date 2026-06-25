namespace WarehouseApi.Models;

public record CreateCategoryDto(string Name);
public record CategoryResponseDto(int Id, string Name);
namespace WarehouseApi.Models;

public record CreateWarehouseDto(string Name);

public record WarehouseResponseDto(int Id, string Name);

namespace WarehouseApi.Models;

public record CreateMovementDto(MovementType Type, int Quantity);

public record MovementResponseDto(int Id, int ProductId, MovementType Type, int Quantity, DateTime CreatedAt);


public record MovementResult(bool Success, string? Error, MovementResponseDto? Movement);
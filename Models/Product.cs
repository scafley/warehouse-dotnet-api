namespace WarehouseApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public string Sku { get; set; } = "";

    public int Quantity { get; set; }

    public int MinQuantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public List<StockMovement> Movements { get; set; } = new();

    public int WarehouseId { get; set; }

    public Warehouse Warehouse { get; set; } = null!;

}
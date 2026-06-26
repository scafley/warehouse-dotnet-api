namespace WarehouseApi.Models;

public class Warehouse
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public List<UserWarehouse> UserWarehouses { get; set; } = new();

    public List<Product> Products { get; set; } = new();
}
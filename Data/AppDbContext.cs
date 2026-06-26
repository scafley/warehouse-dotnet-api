using Microsoft.EntityFrameworkCore;
using WarehouseApi.Models;

namespace WarehouseApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    public DbSet<User> Users => Set<User>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

    public DbSet<UserWarehouse> UserWarehouses => Set<UserWarehouse>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserWarehouse>()
            .HasKey(uw => new { uw.UserId, uw.WarehouseId });
    }
}
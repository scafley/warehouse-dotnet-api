using Microsoft.EntityFrameworkCore;
using WarehouseApi.Models;
using WarehouseApi.Data;

namespace WarehouseApi.Services;

public class ProductService(AppDbContext db) : IProductService
{
    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        return await db.Products
            .OrderBy(p => p.Id)
            .Select(p => new ProductResponseDto(p.Id, p.Name, p.Sku, p.Quantity, p.MinQuantity, p.CreatedAt, p.Category.Name, p.Quantity < p.MinQuantity))
            .ToListAsync();
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        return await db.Products
             .Where(p => p.Id == id)
             .Select(p => new ProductResponseDto(p.Id, p.Name, p.Sku, p.Quantity, p.MinQuantity, p.CreatedAt, p.Category.Name, p.Quantity < p.MinQuantity))
             .FirstOrDefaultAsync();
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product { Name = dto.Name, Sku = dto.Sku, Quantity = dto.Quantity, MinQuantity = dto.MinQuantity, CategoryId = dto.CategoryId };
        db.Products.Add(product);
        await db.SaveChangesAsync();

        var categoryName = await db.Categories.Where(c => c.Id == product.CategoryId).Select(c => c.Name).FirstAsync();

        return new ProductResponseDto(product.Id, product.Name, product.Sku, product.Quantity, product.MinQuantity, product.CreatedAt, categoryName, product.Quantity < product.MinQuantity);
    }

    public async Task<bool> UpdateAsync(int id, CreateProductDto dto)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return false;

        product.Name = dto.Name;
        product.Sku = dto.Sku;
        product.Quantity = dto.Quantity;
        product.MinQuantity = dto.MinQuantity;
        product.CategoryId = dto.CategoryId;


        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return false;

        db.Products.Remove(product);
        await db.SaveChangesAsync();
        return true;
    }

}
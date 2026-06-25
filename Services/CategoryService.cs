using WarehouseApi.Data;
using WarehouseApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WarehouseApi.Services;

public class CategoryService(AppDbContext db) : ICategoryService
{

    public async Task<List<CategoryResponseDto>> GetAllAsync()
    {
        return await db.Categories
        .OrderBy(c => c.Id)
        .Select(c => new CategoryResponseDto(c.Id, c.Name))
        .ToListAsync();
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await db.Categories.FindAsync(id);
        if (category is null) return null;

        return new CategoryResponseDto(category.Id, category.Name);
    }

    public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category { Name = dto.Name };
        db.Categories.Add(category);
        await db.SaveChangesAsync();
        return new CategoryResponseDto(category.Id, category.Name);
    }

    public async Task<bool> UpdateAsync(int id, CreateCategoryDto dto)
    {
        var category = await db.Categories.FindAsync(id);
        if (category is null) return false;

        category.Name = dto.Name;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await db.Categories.FindAsync(id);
        if (category is null) return false;

        db.Categories.Remove(category);
        await db.SaveChangesAsync();
        return true;
    }
}
using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppIdentityContext _context;

    public CategoryRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Category entity)
    {
        await _context.Categories.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Category> GetByIdAsync(Guid id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(k => k.CategoryId == id);
    }

    public async Task<List<Category>> SelectAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task UpdateAsync(Category entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category entity)
    {
        _context.Categories.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Category> GetCategoryByNameAsync(string name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryName == name);
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync()
    {
        return await _context.Categories
            .Include(c => c.SubCategories)
            .ToListAsync();
    }
}
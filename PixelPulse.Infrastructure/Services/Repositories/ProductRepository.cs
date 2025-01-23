using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class ProductRepository : IProductRepository
{
    private AppIdentityContext _context;
    public async Task CreateAsync(Product entity)
    {
        await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(k => k.ProductId == id);
    }

    public async Task<List<Product>> SelectAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task UpdateAsync(Product entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product entity)
    {
        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Product> GetProductByNameAsync(string name)
    {
        return await _context.Products
            .FirstOrDefaultAsync(k => k.Name == name);
    }

    public async Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Products
            .Where(k => k.CategoryId == categoryId)
            .ToListAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class ProductImageRepository : IProductImageRepository
{
    private AppIdentityContext _context;

    public ProductImageRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(ProductImage entity)
    {
        await _context.ProductImages.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ProductImage> GetByIdAsync(Guid id)
    {
        return await _context.ProductImages
            .FirstOrDefaultAsync(k => k.ProductImageId == id);
    }

    public async Task<List<ProductImage>> SelectAsync()
    {
        return await _context.ProductImages.ToListAsync();
    }

    public async Task UpdateAsync(ProductImage entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.ProductImages.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductImage entity)
    {
        _context.ProductImages.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddImagesAsync(List<ProductImage> productImages)
    {
        await _context.ProductImages.AddRangeAsync(productImages);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveImagesAsync(List<ProductImage> productImages)
    {
        _context.ProductImages.RemoveRange(productImages);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProductImage>> GetImagesByProductIdAsync(Guid productId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(k => k.ProductId == productId);

        if (product == null)
            return null;
        
        return await _context.ProductImages
            .Where(k => k.ProductId == productId)
            .ToListAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class ProductSpecificationRepository : IProductSpecificationRepository
{
    private AppIdentityContext _context;

    public ProductSpecificationRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(ProductSpecification entity)
    {
        await _context.ProductSpecifications.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ProductSpecification> GetByIdAsync(Guid id)
    {
        return await _context.ProductSpecifications
            .FirstOrDefaultAsync(k => k.ProductId == id);
    }

    public async Task<List<ProductSpecification>> SelectAsync()
    {
        return await _context.ProductSpecifications.ToListAsync();
    }

    public async Task UpdateAsync(ProductSpecification entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.ProductSpecifications.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductSpecification entity)
    {
        _context.ProductSpecifications.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProductSpecification>> GetByProductIdAsync(Guid productId)
    {
        return await _context.ProductSpecifications
            .Where(k => k.ProductId == productId)
            .ToListAsync();
    }
}
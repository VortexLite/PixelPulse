using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class ProductImageRepository : IProductImageRepository
{
    private readonly AppIdentityContext _context;
    private readonly ILogger<ProductImageRepository> _logger;
    private readonly IMemoryCache _cache;
    
    private const string _productImageCacheKey = "productImage_";

    public ProductImageRepository(AppIdentityContext context, ILogger<ProductImageRepository> logger, IMemoryCache cache)
    {
        _context = context;
        _logger = logger;
        _cache = cache;
    }
    
    public async Task<bool> CreateAsync(ProductImage entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.ProductImages.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_productImageCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<ProductImage> GetById(int id)
    {
        return _context.ProductImages
            .AsNoTracking()
            .Where(i => i.Id == id);
    }

    public IQueryable<ProductImage> GetAll()
    {
        return _context.ProductImages
            .AsNoTracking()
            .Take(100);
    }

    public async Task<bool> UpdateAsync(ProductImage entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.ProductImages.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_productImageCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(ProductImage entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.ProductImages.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_productImageCacheKey + $"{entity.Id}");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteProductImages(List<ProductImage> productImages, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.ProductImages.RemoveRange(productImages);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            foreach (var productImage in productImages)
                _cache.Remove(_productImageCacheKey + $"{productImage.Id}");
            
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<ProductImage> GetImagesByProductId(int productId)
    {
        return _context.ProductImages
            .Where(i => i.ProductId == productId);
    }
}
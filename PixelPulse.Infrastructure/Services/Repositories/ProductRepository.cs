using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppIdentityContext _context;
    private readonly ILogger<ProductRepository> _logger;
    private readonly IMemoryCache _cache;
    
    private const string _productCacheKey = "product_";

    public ProductRepository(AppIdentityContext context, IMemoryCache cache, ILogger<ProductRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Product entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.Products.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_productCacheKey + $"{entity.Id}");
            _cache.Set(_productCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Product> GetById(int id)
    {
        return _context.Products
            .AsNoTracking()
            .Where(i => i.Id == id);
    }

    public IQueryable<Product> GetAll()
    {
        return _context.Products.AsNoTracking();
    }

    public async Task<bool> UpdateAsync(Product entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_productCacheKey + $"{entity.Id}");
            _cache.Set(_productCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Product entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_productCacheKey + $"{entity.Id}");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Product>? GetProductByName(string name)
    {
        return _context.Products
            .AsNoTracking()
            .Where(i => i.Name == name);
    }

    public IQueryable<Product>? GetProductWithImageById(int id)
    {
        return _context.Products
            .Include(p => p.ProductImages)
            .AsNoTracking()
            .Where(p => p.Id == id);
    }

    public IQueryable<Product> GetProductsWithImageAndCategoriesById(int id)
    {
        return _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.Category)
            .Include(p => p.ProductSpecifications)
            .AsNoTracking()
            .Where(p => p.Id == id);
    }

    public IQueryable<Product> GetProductsWithImageTake20()
    {
        return _context.Products
            .Include(p => p.ProductImages)
            .AsNoTracking()
            .Take(20);
    }

    public async Task<bool> DeleteRangeAsync(List<Product> products, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            foreach (var product in products)
            {
                _cache.Remove(_productCacheKey + $"{product.Id}");
            }
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }
}
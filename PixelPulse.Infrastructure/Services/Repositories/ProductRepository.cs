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
    private readonly IMemoryCache _cache;
    private readonly ILogger<ProductRepository> _logger;
    private readonly string _cacheKeyAll = "product_all";
    private readonly string _cacheKeyPrefix = "product_";

    public ProductRepository(AppIdentityContext context, IMemoryCache cache, ILogger<ProductRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task CreateAsync(Product entity, CancellationToken cancellationToken = default)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                await _context.Products.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _cache.Remove(_cacheKeyAll);
                _cache.Set(_cacheKeyPrefix + entity.Id, entity);
                _logger.LogInformation($"Product created: {entity.Id}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError($"Product creation failed, name {entity.Name}. {ex.Message}");
                throw;
            }
        }
    }

    public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyPrefix + id, out Product cachedProduct))
        {
            _logger.LogInformation($"Product found: {_cacheKeyPrefix + id}");
            return cachedProduct;
        }
        else
        {
            cachedProduct = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
            _logger.LogInformation($"Product found: {cachedProduct.Id} {cachedProduct.Name}");

            if (cachedProduct != null)
            {
                _cache.Set(_cacheKeyPrefix + id, cachedProduct, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Product cache created: {_cacheKeyPrefix + id}");
            }
        
            return cachedProduct;
        }
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyAll, out IReadOnlyList<Product> cachedProduct))
        {
            _logger.LogInformation($"Product found: {_cacheKeyAll}");
            return cachedProduct;
        }
        else
        {
            cachedProduct = await _context.Products
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            _logger.LogInformation($"Product founds");

            if (cachedProduct != null)
            {
                _cache.Set(_cacheKeyAll, cachedProduct, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Product cache created: {_cacheKeyAll}");
            }
            
            return cachedProduct;
        }
    }

    public async Task UpdateAsync(Product entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _cache.Set(_cacheKeyPrefix + entity.Id, entity);
            _logger.LogInformation($"Product updated: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Product update failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Product entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _logger.LogInformation($"Product deleted: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Product delete failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task<Product?> GetProductByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyPrefix + name, out Product cachedProduct))
        {
            _logger.LogInformation($"Product found: {_cacheKeyPrefix + name}");
            return cachedProduct;
        }
        else
        {
            cachedProduct = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Name == name, cancellationToken);
            _logger.LogInformation($"Product found: {cachedProduct.Id} {cachedProduct.Name}");

            if (cachedProduct != null)
            {
                _cache.Set(_cacheKeyPrefix + name, cachedProduct, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Product cache created: {_cacheKeyPrefix + name}");
            }
        
            return cachedProduct;
        }
    }

    public async Task<Product?> GetProductWithImageByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyPrefix + id, out Product cachedProduct))
        {
            _logger.LogInformation($"Product found: {_cacheKeyPrefix + id}");
            return cachedProduct;
        }
        else
        {
            cachedProduct = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            
            _logger.LogInformation($"Product found: {cachedProduct.Id} {cachedProduct.Name}");

            if (cachedProduct != null)
            {
                _cache.Set(_cacheKeyPrefix + id, cachedProduct, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Product cache created: {_cacheKeyPrefix + id}");
            }
        
            return cachedProduct;
        }
    }

    public async Task DeleteRangeAsync(List<Product> products, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _logger.LogInformation($"Product deleted: {products.Count}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Product range deletion failed. {ex.Message}");
            throw;
        }
    }
}
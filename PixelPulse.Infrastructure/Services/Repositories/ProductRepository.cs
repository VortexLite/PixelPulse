using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Filter;
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
        return _context.Products
            .AsNoTracking()
            .Take(100);
    }

    public async Task<bool> UpdateAsync(Product entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
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

    public async Task<Product>? GetProductWithImageById(int id)
    {
        return await _context.Products
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == id);
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
                _cache.Remove(_productCacheKey + $"{product.Id}");
            
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Product> GetFilteredProduct(FilterPhoneDTO filter)
    {
        var query = _context.Products.AsQueryable();
        
        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price == filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price == filter.MaxPrice.Value);
        }

        if (filter.Brands != null && filter.Brands.Any())
        {
            query = query.Where(p => filter.Brands.Contains(p.Supplier.Name));
        }

        if (filter.RAMSizes != null && filter.RAMSizes.Any())
        {
            query = query.Where(p => p.ProductSpecifications.Any(s => filter.RAMSizes.Contains(s.Name) && s.Name == "RAM"));
        }

        if (filter.StorageSizes != null && filter.StorageSizes.Any())
        {
            query = query.Where(p => p.ProductSpecifications.Any(s => filter.StorageSizes.Contains(s.Name) && s.Name == "Storage"));
        }

        if (filter.ScreenTypes != null && filter.ScreenTypes.Any())
        {
            query = query.Where(p => p.ProductSpecifications.Any(s => filter.ScreenTypes.Contains(s.Name) && s.Name == "ScreenType"));
        }
        
        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            query = filter.SortBy switch
            {
                "asc" => query.OrderBy(p => p.Price),
                "desc" => query.OrderByDescending(p => p.Price),
                _ => query
            };
        }
        
        return query;
    }

    public IQueryable<Product> GetProductTake50Async(int page = 1)
    {
        int pageSize = 50;
        return _context.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<bool> DeleteProductImage(ProductImage productImage, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_productCacheKey + $"{productImage.Id}");
            
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
                _cache.Remove(_productCacheKey + $"{productImage.Id}");
            
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }
}
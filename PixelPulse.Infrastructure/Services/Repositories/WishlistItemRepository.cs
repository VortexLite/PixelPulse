using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class WishlistItemRepository : IWishlistItemRepository
{
    private readonly AppIdentityContext _context;
    private readonly ILogger<WishlistItemRepository> _logger;
    private readonly IMemoryCache _cache;
    
    private const string _wishlistItemCacheKey = "wishlistItem_";
    
    public WishlistItemRepository(AppIdentityContext context, IMemoryCache cache, ILogger<WishlistItemRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(WishlistItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.WishlistItems.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_wishlistItemCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<WishlistItem> GetById(int id)
    {
        return _context.WishlistItems
            .AsNoTracking()
            .Where(wi => wi.Id == id);
    }

    public IQueryable<WishlistItem> GetAll()
    {
        return _context.WishlistItems
            .AsNoTracking()
            .Take(100);
    }

    public async Task<bool> UpdateAsync(WishlistItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.WishlistItems.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_wishlistItemCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(WishlistItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.WishlistItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_wishlistItemCacheKey + $"{entity.Id}");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<WishlistItem>? GetWishlistItemByWishlistAndProductId(int cartId, int productId)
    {
        return _context.WishlistItems
            .Where(ci => ci.WishlistId == cartId
                         && ci.ProductId == productId);

    }

    public IQueryable<WishlistItem>? GetWishlistItemsByWishlistId(int wishlistId)
    {
        return _context.WishlistItems
            .Where(wi => wi.WishlistId == wishlistId);
    }
}
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
    private readonly IMemoryCache _cache;
    private readonly ILogger<WishlistItemRepository> _logger;
    private readonly string _cacheKeyAll = "wishlistItem_all";
    private readonly string _cacheKeyPrefix = "wishlistItem_";

    public WishlistItemRepository(AppIdentityContext context, IMemoryCache cache, ILogger<WishlistItemRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task CreateAsync(WishlistItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.WishlistItems.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Set(_cacheKeyPrefix + entity.Id, entity);
            _logger.LogInformation($"WishlistItem created: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError($"WishlistItem creation failed. {ex.Message}");
            throw;
        }
    }

    public async Task<WishlistItem> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyPrefix + id, out WishlistItem cached))
        {
            _logger.LogInformation($"Product found: {_cacheKeyPrefix + id}");
            return cached;
        }
        else
        {
            cached = await _context.WishlistItems
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
            _logger.LogInformation($"WishlistItem found: {cached.Id}");

            if (cached != null)
            {
                _cache.Set(_cacheKeyPrefix + id, cached, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"WishlistItem cache created: {_cacheKeyPrefix + id}");
            }
        
            return cached;
        }
    }

    public async Task<IReadOnlyList<WishlistItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyAll, out List<WishlistItem> cached))
        {
            _logger.LogInformation($"Product found: {_cacheKeyAll}");
            return cached;
        }
        else
        {
            cached = await _context.WishlistItems
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            _logger.LogInformation($"WishlistItem founds");

            if (cached != null)
            {
                _cache.Set(_cacheKeyAll, cached, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"WishlistItem cache created: {_cacheKeyAll}");
            }
            
            return cached;
        }
    }

    public async Task UpdateAsync(WishlistItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.WishlistItems.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _cache.Set(_cacheKeyPrefix + entity.Id, entity);
            _logger.LogInformation($"WishlistItem updated: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"WishlistItem update failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(WishlistItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.WishlistItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _logger.LogInformation($"WishlistItem deleted: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"WishlistItem delete failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task<WishlistItem?> GetWishlistItemByWishlistAndProductId(int cartId, int productId)
    {
        var wishlistItem = await _context.WishlistItems
            .Where(ci => ci.WishlistId == cartId)
            .FirstOrDefaultAsync(ci => ci.ProductId == productId);
        
        return wishlistItem;
    }
}
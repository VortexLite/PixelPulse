using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly AppIdentityContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<WishlistRepository> _logger;
    private readonly string _cacheKeyAll = "wishlist_all";
    private readonly string _cacheKeyPrefix = "wishlist_";

    public WishlistRepository(AppIdentityContext context, IMemoryCache cache, ILogger<WishlistRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task CreateAsync(Wishlist entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.Wishlists.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Set(_cacheKeyPrefix + entity.Id, entity);
            _logger.LogInformation($"Wishlist created: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError($"Wishlist creation failed. {ex.Message}");
            throw;
        }
    }

    public async Task<Wishlist> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyPrefix + id, out Wishlist cached))
        {
            _logger.LogInformation($"Wishlist found: {_cacheKeyPrefix + id}");
            return cached;
        }
        else
        {
            cached = await _context.Wishlists
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
            _logger.LogInformation($"Wishlist found: {cached.Id}");

            if (cached != null)
            {
                _cache.Set(_cacheKeyPrefix + id, cached, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Wishlist cache created: {_cacheKeyPrefix + id}");
            }
        
            return cached;
        }
    }

    public async Task<IReadOnlyList<Wishlist>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyAll, out IReadOnlyList<Wishlist> cached))
        {
            _logger.LogInformation($"Product found: {_cacheKeyAll}");
            return cached;
        }
        else
        {
            cached = await _context.Wishlists
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            _logger.LogInformation($"Product founds");

            if (cached != null)
            {
                _cache.Set(_cacheKeyAll, cached, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Wishlist cache created: {_cacheKeyAll}");
            }
            
            return cached;
        }
    }

    public async Task UpdateAsync(Wishlist entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Wishlists.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _cache.Set(_cacheKeyPrefix + entity.Id, entity);
            _logger.LogInformation($"Wishlist updated: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Wishlist update failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Wishlist entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Wishlists.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _logger.LogInformation($"Wishlist deleted: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Wishlist delete failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task<Wishlist?> FindWishlistByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var wishlist = await _context.Wishlists
            .FirstOrDefaultAsync(i => i.AppUserId == userId, cancellationToken);
        
        return wishlist;
    }
}
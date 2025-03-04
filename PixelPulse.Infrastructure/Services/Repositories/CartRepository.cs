using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppIdentityContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CartRepository> _logger;
    private readonly string _cacheKeyAll = "cart_all";
    private readonly string _cacheKeyPrefix = "cart_";

    public CartRepository(AppIdentityContext context, IMemoryCache cache, ILogger<CartRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task CreateAsync(Cart entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.Carts.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Set(_cacheKeyPrefix + entity.Id, entity);
            _logger.LogInformation($"Cart created: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError($"Cart creation failed, name {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task<Cart> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyPrefix + id, out Cart cachedCart))
        {
            _logger.LogInformation($"Cart found: {_cacheKeyPrefix + id}");
            return cachedCart;
        }
        else
        {
            cachedCart = await _context.Carts
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
            _logger.LogInformation($"Cart found: {cachedCart.Id}");

            if (cachedCart != null)
            {
                _cache.Set(_cacheKeyPrefix + id, cachedCart, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Cart cache created: {_cacheKeyPrefix + id}");
            }
        
            return cachedCart;
        }
    }

    public async Task<IReadOnlyList<Cart>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyAll, out IReadOnlyList<Cart> cachedCart))
        {
            _logger.LogInformation($"Product found: {_cacheKeyAll}");
            return cachedCart;
        }
        else
        {
            cachedCart = await _context.Carts
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            _logger.LogInformation($"Product founds");

            if (cachedCart != null)
            {
                _cache.Set(_cacheKeyAll, cachedCart, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Cart cache created: {_cacheKeyAll}");
            }
            
            return cachedCart;
        }
    }

    public async Task UpdateAsync(Cart entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Carts.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _cache.Set(_cacheKeyPrefix + entity.Id, entity);
            _logger.LogInformation($"Cart updated: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Cart update failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Cart entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Carts.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _logger.LogInformation($"Cart deleted: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Cart delete failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task<Cart> FindCartByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var cart = await _context.Carts
            .FirstOrDefaultAsync(i => i.AppUserId == userId, cancellationToken);
        _logger.LogInformation($"Cart found: {cart.Id}");
        
         return cart;
    }
}
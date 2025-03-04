using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private readonly AppIdentityContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CartItemRepository> _logger;
    private readonly string _cacheKeyAll = "cartItem_all";
    private readonly string _cacheKeyPrefix = "cartItem_";

    public CartItemRepository(AppIdentityContext context, IMemoryCache cache, ILogger<CartItemRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task CreateAsync(CartItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.CartItems.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Set(_cacheKeyPrefix + entity.Id, entity);
            _logger.LogInformation($"CartItem created: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError($"CartItem creation failed, name {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task<CartItem> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyPrefix + id, out CartItem cached))
        {
            _logger.LogInformation($"Product found: {_cacheKeyPrefix + id}");
            return cached;
        }
        else
        {
            cached = await _context.CartItems
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
            _logger.LogInformation($"Product found: {cached.Id}");

            if (cached != null)
            {
                _cache.Set(_cacheKeyPrefix + id, cached, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Product cache created: {_cacheKeyPrefix + id}");
            }
        
            return cached;
        }
    }

    public async Task<IReadOnlyList<CartItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyAll, out IReadOnlyList<CartItem> cached))
        {
            _logger.LogInformation($"Product found: {_cacheKeyAll}");
            return cached;
        }
        else
        {
            cached = await _context.CartItems
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            _logger.LogInformation($"Product founds");

            if (cached != null)
            {
                _cache.Set(_cacheKeyAll, cached, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"CartItem cache created: {_cacheKeyAll}");
            }
            
            return cached;
        }
    }

    public async Task UpdateAsync(CartItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.CartItems.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _cache.Set(_cacheKeyPrefix + entity.Id, entity);
            _logger.LogInformation($"CartItem updated: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"CartItem update failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(CartItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.CartItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _logger.LogInformation($"CartItem deleted: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"CartItem delete failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task<CartItem?> GetCartItemByCartAndProductId(int cartId, int productId)
    {
        var cartItem = await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .FirstOrDefaultAsync(ci => ci.ProductId == productId);
        
        return cartItem;
    }
}
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
    private readonly ILogger<CartRepository> _logger;
    private readonly IMemoryCache _cache;
    
    private const string _cartCacheKey = "cart_";

    public CartRepository(AppIdentityContext context, IMemoryCache cache, ILogger<CartRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Cart entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.Carts.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_cartCacheKey + $"{entity.Id}", entity, TimeSpan.FromHours(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Cart> GetById(int id)
    {
        return _context.Carts
            .AsNoTracking()
            .Where(c => c.Id == id);
    }

    public IQueryable<Cart> GetAll()
    {
        return _context.Carts
            .AsNoTracking();
    }

    public async Task<bool> UpdateAsync(Cart entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Carts.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cartCacheKey + $"{entity.Id}");
            _cache.Set(_cartCacheKey + $"{entity.Id}", entity, TimeSpan.FromHours(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Cart entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Carts.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_cartCacheKey + $"{entity.Id}");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Cart> FindCartByUserId(string userId)
    {
        return _context.Carts
            .Where(i => i.AppUserId == userId);
    }
}
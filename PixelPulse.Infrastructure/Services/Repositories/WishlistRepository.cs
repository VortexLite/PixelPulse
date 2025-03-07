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
    private readonly ILogger<WishlistRepository> _logger;
    private readonly IMemoryCache _cache;
    
    private const string _wishlistCacheKey = "wishlist_";

    public WishlistRepository(AppIdentityContext context, IMemoryCache cache, ILogger<WishlistRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Wishlist entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.Wishlists.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_wishlistCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Wishlist> GetById(int id)
    {
        return _context.Wishlists
            .AsNoTracking()
            .Where(i => i.Id == id);
    }

    public IQueryable<Wishlist> GetAll()
    {
        return _context.Wishlists.AsNoTracking();
    }

    public async Task<bool> UpdateAsync(Wishlist entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Wishlists.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_wishlistCacheKey + $"{entity.Id}");
            _cache.Set(_wishlistCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Wishlist entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Wishlists.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_wishlistCacheKey + $"{entity.Id}");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Wishlist>? FindWishlistByUserId(string userId)
    {
        return _context.Wishlists
            .Where(i => i.AppUserId == userId);
    }
}
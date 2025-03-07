using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private readonly AppIdentityContext _context;
    private readonly ILogger<CartItemRepository> _logger;
    private readonly IMemoryCache _cache;
    
    private const string _cartItemCacheKey = "cartItem_";
    
    public CartItemRepository(AppIdentityContext context, IMemoryCache cache, ILogger<CartItemRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(CartItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.CartItems.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_cartItemCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<CartItem> GetById(int id)
    {
        return _context.CartItems
            .AsNoTracking()
            .Where(i => i.Id == id);
    }


    public IQueryable<CartItem> GetAll()
    {
        return _context.CartItems.AsNoTracking();
    }

    public async Task<bool> UpdateAsync(CartItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.CartItems.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_cartItemCacheKey + $"{entity.Id}");
            _cache.Set(_cartItemCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(CartItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.CartItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_cartItemCacheKey + $"{entity.Id}");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }
    
    public IQueryable<CartItem>? GetCartItemsByCartAndProductId(int cartId, int productId)
    {
        return  _context.CartItems
            .Where(ci => ci.CartId == cartId
                         && ci.ProductId == productId);
    }

    public IQueryable<CartItem>? GetCartItemsByCartId(int cartId)
    {
        return _context.CartItems
            .Where(ci => ci.CartId == cartId);
    }

    

    public async Task<bool> DeleteCartItemsAsync(List<CartItemDTO> cartItemDtos, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.CartItems.RemoveRange(cartItemDtos.Adapt<CartItem>());
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            foreach (var cartItem in cartItemDtos)
            {
                _cache.Remove(_cartItemCacheKey + $"{cartItem.Id}");
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
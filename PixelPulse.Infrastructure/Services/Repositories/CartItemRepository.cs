using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private AppIdentityContext _context;

    public CartItemRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(CartItem entity)
    {
        await _context.CartItems.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<CartItem> GetByIdAsync(Guid id)
    {
        return await _context.CartItems.FirstOrDefaultAsync(k => k.CartItemId == id);
    }

    public async Task<List<CartItem>> SelectAsync()
    {
        return await _context.CartItems.ToListAsync();
    }

    public async Task UpdateAsync(CartItem entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.CartItems.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CartItem entity)
    {
        _context.CartItems.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddItemsAsync(List<CartItem> cartItems)
    {
        await _context.CartItems.AddRangeAsync(cartItems);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveItemsAsync(List<CartItem> cartItem)
    {
        _context.CartItems.RemoveRange(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CartItem>> GetAllItemsByUserIdAsync(string userId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(k => k.AppUserId == userId);
        if (cart == null)
            return null;
        
        return await _context.CartItems
            .Where(k => k.CartId == cart.CartId)
            .ToListAsync();
        
    }
}
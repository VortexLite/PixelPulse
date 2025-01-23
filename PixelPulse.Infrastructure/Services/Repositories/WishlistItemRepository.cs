using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class WishlistItemRepository : IWishlistItemRepository
{
    private AppIdentityContext _context;

    public WishlistItemRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(WishlistItem entity)
    {
        await _context.WishlistItems.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<WishlistItem> GetByIdAsync(Guid id)
    {
        return await _context.WishlistItems
            .FirstOrDefaultAsync(k => k.WishlistItemId == id);
    }

    public async Task<List<WishlistItem>> SelectAsync()
    {
        return await _context.WishlistItems.ToListAsync();
    }

    public async Task UpdateAsync(WishlistItem entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.WishlistItems.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(WishlistItem entity)
    {
        _context.WishlistItems.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<WishlistItem>> GetWishlistItemByUserIdAsync(string userId)
    {
        var wishlist = await _context.Wishlists
            .FirstOrDefaultAsync(k => k.AppUserId == userId);

        if (wishlist == null)
            return null;

        return await _context.WishlistItems
            .Where(k => k.WishlistId == wishlist.WishlistId)
            .ToListAsync();
    }
}
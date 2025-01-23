using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private AppIdentityContext _context;

    public WishlistRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Wishlist entity)
    {
        await _context.Wishlists.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Wishlist> GetByIdAsync(Guid id)
    {
        return await _context.Wishlists
            .FirstOrDefaultAsync(k => k.WishlistId == id);
    }

    public async Task<List<Wishlist>> SelectAsync()
    {
        return await _context.Wishlists.ToListAsync();
    }

    public async Task UpdateAsync(Wishlist entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Wishlists.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Wishlist entity)
    {
        _context.Wishlists.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
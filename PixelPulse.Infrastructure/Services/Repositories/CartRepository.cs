using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class CartRepository : ICartRepository
{
    private AppIdentityContext _context;

    public CartRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Cart entity)
    {
        await _context.Carts.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Cart> GetByIdAsync(Guid id)
    {
        return await _context.Carts.FirstOrDefaultAsync(k => k.CartId == id);
    }

    public async Task<List<Cart>> SelectAsync()
    {
        return await _context.Carts.ToListAsync();
    }

    public async Task UpdateAsync(Cart entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Cart entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Cart> GetCartByUserIdAsync(string userId)
    {
        return await _context.Carts
            .FirstOrDefaultAsync(k => k.AppUserId == userId);
    }
}
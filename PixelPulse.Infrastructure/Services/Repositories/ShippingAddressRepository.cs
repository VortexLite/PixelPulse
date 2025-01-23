using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class ShippingAddressRepository : IShippingAddressRepository
{
    private AppIdentityContext _context;

    public ShippingAddressRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(ShippingAddress entity)
    {
        await _context.ShippingAddresses.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ShippingAddress> GetByIdAsync(Guid id)
    {
        return await _context.ShippingAddresses
            .FirstOrDefaultAsync(k => k.ShippingAddressId == id);
    }

    public async Task<List<ShippingAddress>> SelectAsync()
    {
        return await _context.ShippingAddresses.ToListAsync();
    }

    public async Task UpdateAsync(ShippingAddress entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.ShippingAddresses.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ShippingAddress entity)
    {
        _context.ShippingAddresses.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ShippingAddress?> GetAddressByUserIdAsync(string userId)
    {
        return await _context.ShippingAddresses
            .FirstOrDefaultAsync(k => k.AppUserId == userId);
    }
}
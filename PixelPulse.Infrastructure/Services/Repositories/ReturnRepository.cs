using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class ReturnRepository : IReturnRepository
{
    private AppIdentityContext _context;

    public ReturnRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Return entity)
    {
        await _context.Returns.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Return> GetByIdAsync(Guid id)
    {
        return await _context.Returns
            .FirstOrDefaultAsync(k => k.OrderId == id);
    }

    public async Task<List<Return>> SelectAsync()
    {
        return await _context.Returns.ToListAsync();
    }

    public async Task UpdateAsync(Return entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Return entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Return> GetByOrderIdAsync(Guid orderId)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(k => k.OrderId == orderId);
        
        if (order == null)
            return null;
        
        return await _context.Returns
            .FirstOrDefaultAsync(k => k.OrderId == order.OrderId);
    }
}
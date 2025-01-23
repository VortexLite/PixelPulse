using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class ReturnItemRepository : IReturnItemRepository
{
    private AppIdentityContext _context;

    public ReturnItemRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(ReturnItem entity)
    {
        await _context.ReturnItems.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ReturnItem> GetByIdAsync(Guid id)
    {
        return await _context.ReturnItems
            .FirstOrDefaultAsync(k => k.ReturnItemId == id);
    }

    public async Task<List<ReturnItem>> SelectAsync()
    {
        return await _context.ReturnItems.ToListAsync();
    }

    public async Task UpdateAsync(ReturnItem entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.ReturnItems.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ReturnItem entity)
    {
        _context.ReturnItems.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddReturnItemsAsync(List<ReturnItem> returnItems)
    {
        await _context.ReturnItems.AddRangeAsync(returnItems);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveReturnItemsAsync(List<ReturnItem> returnItems)
    {
        _context.ReturnItems.RemoveRange(returnItems);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ReturnItem>> GetReturnItemsByUserIdAsync(string userId)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(k => k.AppUserId == userId);
        
        if (order == null)
            return null;
        
        var returns = await _context.Returns
            .FirstOrDefaultAsync(k => k.OrderId == order.OrderId);
        
        if (returns == null)
            return null;
        
        return await _context.ReturnItems
            .Where(k => k.ReturnId == returns.ReturnId)
            .ToListAsync();
    }

    public async Task<List<ReturnItem>> GetReturnItemsByReturnIdAsync(Guid returnId)
    {
        return await _context.ReturnItems
            .Where(k => k.ReturnId == returnId)
            .ToListAsync();
    }
}
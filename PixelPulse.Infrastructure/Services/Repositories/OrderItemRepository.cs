using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private AppIdentityContext _context;

    public OrderItemRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(OrderItem entity)
    {
        _context.OrderItems.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<OrderItem> GetByIdAsync(Guid id)
    {
        return await _context.OrderItems
            .FirstOrDefaultAsync(k => k.OrderId == id);
    }

    public async Task<List<OrderItem>> SelectAsync()
    {
        return await _context.OrderItems.ToListAsync();
    }

    public async Task UpdateAsync(OrderItem entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.OrderItems.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(OrderItem entity)
    {
        _context.OrderItems.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<OrderItem>> GetOrderItemsByUserIdAsync(string userId)
    {
        var user = await _context.Users.
            FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user == null)
            return null;
        
        var order = await _context.Orders
            .FirstOrDefaultAsync(k => k.AppUserId == userId);
        
        if (order == null)
            return null;

        return await _context.OrderItems
            .Where(k => k.OrderId == order.OrderId)
            .ToListAsync();
    }

    public async Task AddOrdersItemAsync(List<OrderItem> orderItems)
    {
        await _context.OrderItems.AddRangeAsync(orderItems);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveOrdersItemAsync(List<OrderItem> orderItems)
    {
        _context.OrderItems.RemoveRange(orderItems);
        await _context.SaveChangesAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class OrderRepository : IOrderRepository
{
    private AppIdentityContext _context;

    public OrderRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Order entity)
    {
        await _context.Orders.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(k => k.OrderId == id);
    }

    public async Task<List<Order>> SelectAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task UpdateAsync(Order entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Orders.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Order entity)
    {
        _context.Orders.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Order> GetOrdersByUserIdAsync(string userId)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(k => k.AppUserId == userId);
    }
}
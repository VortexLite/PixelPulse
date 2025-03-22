using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppIdentityContext _context;
    private readonly ILogger<OrderRepository> _logger;
    private readonly IMemoryCache _cache;
    
    private const string _orderCacheKey = "order_";

    public OrderRepository(AppIdentityContext context, IMemoryCache cache, ILogger<OrderRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.Orders.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_orderCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Order> GetById(int id)
    {
        return _context.Orders
            .AsNoTracking()
            .Where(i => i.Id == id);
    }

    public IQueryable<Order> GetAll()
    {
        return _context.Orders
            .AsNoTracking()
            .Take(100);
    }

    public async Task<bool> UpdateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Orders.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_orderCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Order entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Orders.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_orderCacheKey + $"{entity.Id}");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Order> GetOrdersByUserId(string userId)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.ProductImages)
            .Include(o => o.Shipping)
            .Where(o => o.AppUserId == userId);
    }

    public IQueryable<Order> GetOrderWithItems(int page)
    {
        int pageSize = 50;
        return _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Orders.CountAsync();
    }

    public IQueryable<Order> GetOrderItemsById(int orderId)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.Id == orderId);
            
    }
}
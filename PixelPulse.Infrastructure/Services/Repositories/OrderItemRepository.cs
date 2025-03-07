using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly AppIdentityContext _context;
    private readonly ILogger<OrderItemRepository> _logger;
    private readonly IMemoryCache _cache;
    
    private const string _orderItemCacheKey = "orderItem_";

    public OrderItemRepository(AppIdentityContext context, IMemoryCache cache, ILogger<OrderItemRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(OrderItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.OrderItems.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_orderItemCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<OrderItem> GetById(int id)
    {
        return _context.OrderItems
            .AsNoTracking()
            .Where(i => i.Id == id);
    }

    public IQueryable<OrderItem> GetAll()
    {
        return _context.OrderItems.AsNoTracking();
    }

    public async Task<bool> UpdateAsync(OrderItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.OrderItems.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            _cache.Remove(_orderItemCacheKey + $"{entity.Id}");
            _cache.Set(_orderItemCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(OrderItem entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.OrderItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_orderItemCacheKey + $"{entity.Id}");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> AddRangeAsync(List<OrderItem> orderItems, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.OrderItems.AddRangeAsync(orderItems, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            foreach (var orderItem in orderItems)
            {
                _cache.Set(_orderItemCacheKey + $"{orderItem.Id}", orderItem, TimeSpan.FromMinutes(30));
            }
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class ShippingRepository : IShippingRepository
{
    private readonly AppIdentityContext _context;
    private readonly ILogger<ShippingRepository> _logger;
    private readonly IMemoryCache _cache;
    
    private const string _shippingCacheKey = "shipping_";

    public ShippingRepository(AppIdentityContext context, ILogger<ShippingRepository> logger, IMemoryCache cache)
    {
        _context = context;
        _logger = logger;
        _cache = cache;
    }

    public async Task<bool> CreateAsync(Shipping entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.Shippings.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_shippingCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Shipping> GetById(int id)
    {
        return _context.Shippings
            .AsNoTracking()
            .Where(i => i.Id == id);
    }

    public IQueryable<Shipping> GetAll()
    {
        return _context.Shippings
            .AsNoTracking()
            .Take(100);
    }

    public async Task<bool> UpdateAsync(Shipping entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Shippings.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_shippingCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Shipping entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Shippings.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_shippingCacheKey + $"{entity.Id}");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }
}
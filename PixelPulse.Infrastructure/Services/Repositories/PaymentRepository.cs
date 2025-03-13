using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppIdentityContext _context;
    private readonly ILogger<PaymentRepository> _logger;
    private readonly IMemoryCache _cache;
    
    private const string _paymentCacheKey = "payment_";

    public PaymentRepository(AppIdentityContext context, ILogger<PaymentRepository> logger, IMemoryCache cache)
    {
        _context = context;
        _logger = logger;
        _cache = cache;
    }

    public async Task<bool> CreateAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.Payments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_paymentCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<Payment> GetById(int id)
    {
        return _context.Payments
            .AsNoTracking()
            .Where(i => i.Id == id);
    }

    public IQueryable<Payment> GetAll()
    {
        return _context.Payments
            .AsNoTracking()
            .Take(100);
    }

    public async Task<bool> UpdateAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Payments.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Set(_paymentCacheKey + $"{entity.Id}", entity, TimeSpan.FromMinutes(30));
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Payments.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            _cache.Remove(_paymentCacheKey + $"{entity.Id}");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }
}
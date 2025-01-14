using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Content.Services.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _dbContext;

    public BaseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Create(T entity)
    {
        await _dbContext.AddAsync(entity);
        entity.DateCreated = DateTimeOffset.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _dbContext.Update(entity);
        entity.DateUpdated = DateTimeOffset.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _dbContext.Remove(entity);
        entity.DateDeleted = DateTimeOffset.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<T> Get(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<T>> GetAll(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }
}
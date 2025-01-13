using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Content.Services.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext DbContext;

    public BaseRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public async Task Create(T entity)
    {
        await DbContext.AddAsync(entity);
        entity.DateCreated = DateTimeOffset.UtcNow;
        await DbContext.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        DbContext.Update(entity);
        entity.DateUpdated = DateTimeOffset.UtcNow;
        await DbContext.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        DbContext.Remove(entity);
        entity.DateDeleted = DateTimeOffset.UtcNow;
        await DbContext.SaveChangesAsync();
    }

    public async Task<T> Get(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<T>> GetAll(CancellationToken cancellationToken)
    {
        return await DbContext.Set<T>().ToListAsync(cancellationToken);
    }
}
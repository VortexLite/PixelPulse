using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IBaseRepository<T>
{
    Task CreateAsync(T entity);
    Task<T> GetByIdAsync(Guid id);
    Task<List<T>> SelectAsync();
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
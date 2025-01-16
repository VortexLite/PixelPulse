using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IBaseRepository<T>
{
    Task CreateAsync(T entity);
    Task GetAsync(Guid id);
    Task<List<T>> SelectAsync();
    Task<T> Update(T entity);
    Task Delete(T entity);
}
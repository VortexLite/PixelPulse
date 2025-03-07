namespace PixelPulse.Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<bool> CreateAsync(T entity, CancellationToken cancellationToken = default);
    IQueryable<T> GetById(int id);
    IQueryable<T> GetAll();
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
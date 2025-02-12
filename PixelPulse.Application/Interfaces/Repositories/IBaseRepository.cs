namespace PixelPulse.Application.Interfaces.Repositories;

public interface IBaseRepository<T>
{
    Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
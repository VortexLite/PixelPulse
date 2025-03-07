using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<bool> CreateRangeAsync(List<Category> categories, CancellationToken cancellationToken = default);
    Task<bool> DeleteRangeAsync(List<Category> categories, CancellationToken cancellationToken = default);

    
}
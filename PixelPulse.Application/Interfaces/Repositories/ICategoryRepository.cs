using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task CreateRangeAsync(List<Category> categories, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(List<Category> categories, CancellationToken cancellationToken = default);

    
}
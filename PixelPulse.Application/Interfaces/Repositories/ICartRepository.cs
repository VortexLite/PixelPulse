using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface ICartRepository : IBaseRepository<Cart>
{
    Task<Cart> FindCartByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
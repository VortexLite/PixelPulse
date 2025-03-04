using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IWishlistRepository : IBaseRepository<Wishlist>
{
    Task<Wishlist?> FindWishlistByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
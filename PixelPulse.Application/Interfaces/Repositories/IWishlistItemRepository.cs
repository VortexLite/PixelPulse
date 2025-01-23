using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IWishlistItemRepository : IBaseRepository<WishlistItem>
{
    Task<List<WishlistItem>> GetWishlistItemByUserIdAsync(string userId);
}
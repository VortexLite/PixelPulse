using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IWishlistItemRepository : IBaseRepository<WishlistItem>
{
    Task<WishlistItem?> GetWishlistItemByWishlistAndProductId(int cartId, int productId);
}
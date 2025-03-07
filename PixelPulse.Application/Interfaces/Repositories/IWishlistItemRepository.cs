using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IWishlistItemRepository : IBaseRepository<WishlistItem>
{
    IQueryable<WishlistItem>? GetWishlistItemByWishlistAndProductId(int cartId, int productId);
    IQueryable<WishlistItem>? GetWishlistItemsByWishlistId(int wishlistId);
}
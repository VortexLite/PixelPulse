using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IWishlistRepository : IBaseRepository<Wishlist>
{
    IQueryable<Wishlist>? FindWishlistByUserId(string userId);
}
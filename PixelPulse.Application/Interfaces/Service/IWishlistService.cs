using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.DTOs.Wishlist;

namespace PixelPulse.Application.Interfaces.Service;

public interface IWishlistService
{
    Task<List<WishlistItemDTO>?> GetWishlistByRefreshTokenAsync(string token);
    Task<bool> AddWishlistItemAsync(ProductIdTokenRequest request);
}
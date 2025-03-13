using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Product;

namespace PixelPulse.Application.Interfaces.Service;

public interface ICartService
{
    Task<List<CartItemDTO>?> GetCartsByRefreshTokenAsync(string token);
    Task<bool> DellCartItemAsync(int cartItemId);
    Task<bool> AddCartItemAsync(ProductIdTokenRequest request);
    Task<bool> UpdateQuantityAsync(UpdateCartItemRequest request);
    
}
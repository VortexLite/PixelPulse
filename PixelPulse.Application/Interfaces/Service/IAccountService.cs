using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Application.DTOs.Review;
using PixelPulse.Application.DTOs.Wishlist;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Service;

public interface IAccountService
{
    Task<AppUser?> FindUserByRefreshToken(string token);
    Task<AppUser> UpdateAccount(UserUpdateRequest userUpdateRequest);
    Task<List<OrderDTO>> GetOrdersByRefreshToken(string token);
    Task<List<CartItemDTO>> GetCartsByRefreshToken(string token);
    Task<List<WishlistItemDTO>> GetWishlistByRefreshToken(string token);
    Task<List<ReviewDTO>> GetReviewByRefreshToken(string token);
}
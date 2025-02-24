using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Application.DTOs.Review;
using PixelPulse.Application.DTOs.Wishlist;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Accounts;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppIdentityContext _context;

    public AccountService(ILogger<AccountService> logger, UserManager<AppUser> userManager, AppIdentityContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _context = context;
    }

    public async Task<AppUser?> FindUserByRefreshToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        return await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens
                .Any(t => t.Token == token));
    }

    public async Task<AppUser> UpdateAccount(UserUpdateRequest userUpdateRequest)
    {
        var user = await FindUserByRefreshToken(userUpdateRequest.RefreshToken);
        
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            user.Email = userUpdateRequest.AppUser.Email;
            user.UserName = userUpdateRequest.AppUser.UserName;
            user.FirstName = userUpdateRequest.AppUser.FirstName;
            user.LastName = userUpdateRequest.AppUser.LastName;
            user.Address = userUpdateRequest.AppUser.Address;
            user.PhoneNumber = userUpdateRequest.AppUser.PhoneNumber;
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, $"Error saving personal information {userUpdateRequest.AppUser.Email}");
            throw;
        }

        return user;
    }

    public async Task<List<OrderDTO>> GetOrdersByRefreshToken(string token)
    {
        var user = await FindUserByRefreshToken(token);
        var ordersDTO = await _context.Orders
            .Where(o => o.AppUserId == user.Id)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ProjectToType<OrderDTO>()
            .ToListAsync();
        
        return ordersDTO;
    }

    public async Task<List<CartItemDTO>> GetCartsByRefreshToken(string token)
    {
        var user = await FindUserByRefreshToken(token);
        var cart = await _context.Carts.FirstOrDefaultAsync(k => k.AppUserId == user.Id);
        var cartItemsDtos = await _context.CartItems
            .Where(ci => ci.CartId == cart.Id)
            .ProjectToType<CartItemDTO>()
            .ToListAsync();
        
        return cartItemsDtos;
    }

    public async Task<List<WishlistItemDTO>> GetWishlistByRefreshToken(string token)
    {
        var user = await FindUserByRefreshToken(token);
        var wishlist = await _context.Wishlists.FirstOrDefaultAsync(k => k.AppUserId == user.Id);
        var wishListItemsDtos = await _context.WishlistItems
            .Where(wi => wi.WishlistId == wishlist.Id)
            .ProjectToType<WishlistItemDTO>()
            .ToListAsync();
        
        return wishListItemsDtos;
    }

    public async Task<List<ReviewDTO>> GetReviewByRefreshToken(string token)
    {
        var user = await FindUserByRefreshToken(token);
        var reviewsDtos = await _context.Reviews
            .Where(a => a.AppUserId == user.Id)
            .ProjectToType<ReviewDTO>()
            .ToListAsync();
        
        return reviewsDtos;
    }
}
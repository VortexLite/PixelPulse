using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.DTOs.Wishlist;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Service.Wishlist;

public class WishlistService : IWishlistService
{
    private readonly ILogger<WishlistService> _logger;
    private readonly IUserService _userService;
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IWishlistItemRepository _wishlistItemRepository;
    private readonly IMemoryCache _cache;
    
    private const string _wishlistCacheKey = "wishlist_";
    private const string _wishlistItemCacheKey = "wishlistItem_";

    public WishlistService(ILogger<WishlistService> logger, IUserService userService, IWishlistRepository wishlistRepository, IWishlistItemRepository wishlistItemRepository, IMemoryCache cache)
    {
        _logger = logger;
        _userService = userService;
        _wishlistRepository = wishlistRepository;
        _wishlistItemRepository = wishlistItemRepository;
        _cache = cache;
    }

    public async Task<List<WishlistItemDTO>?> GetWishlistByRefreshTokenAsync(string token)
    {
        var user = await _userService.FindUserByRefreshToken(token);
        if (!_cache.TryGetValue(_wishlistItemRepository + "wishlistItems" + $"{user.Id}", out List<WishlistItemDTO> cachedWishlist))
        {
            var wishlistId = await _wishlistRepository
                .FindWishlistByUserId(user.Id)
                .Select(w => w.Id)
                .FirstOrDefaultAsync();
            
            var wishListItemsQuery = _wishlistItemRepository.GetWishlistItemsByWishlistId(wishlistId);
            
            cachedWishlist = await wishListItemsQuery
                .ProjectToType<WishlistItemDTO>()
                .ToListAsync();
            
            _cache.Set(_wishlistItemRepository + "wishlistItems" + $"{user.Id}", cachedWishlist, TimeSpan.FromMinutes(30));
            foreach (var wishlistItem in cachedWishlist)
                _cache.Set(_wishlistItemRepository + $"{wishlistItem.Id}", wishlistItem, TimeSpan.FromMinutes(30));
        }
        
        return cachedWishlist;
    }

    public async Task<bool> AddWishlistItemAsync(ProductIdTokenRequest request)
    {
        var user = await _userService.FindUserByRefreshToken(request.RefreshToken);
        if (user == null)
            return false;
        
        var wishlist = await _wishlistRepository
            .FindWishlistByUserId(user.Id)
            .FirstOrDefaultAsync()
            ?? new Domain.Entities.Wishlist { AppUserId = user.Id }; 
        if (wishlist.Id == 0)
            await _wishlistRepository.CreateAsync(wishlist);

        var wishlistItem = await _wishlistItemRepository
            .GetWishlistItemByWishlistAndProductId(wishlist.Id, request.ProductId)
            .FirstOrDefaultAsync()
            ?? new WishlistItem
            {
                WishlistId = wishlist.Id,
                ProductId = request.ProductId
            };
        if (wishlistItem.WishlistId == 0)
            await _wishlistItemRepository.CreateAsync(wishlistItem);
        
        return true;
    }

    public async Task<bool> RemoveWishlistItemAsync(int wishlistItemId)
    {
        var wishlistItem = await _wishlistItemRepository
            .GetById(wishlistItemId)
            .FirstOrDefaultAsync();
        if (wishlistItem == null)
            return false;
        
        return await _wishlistItemRepository.DeleteAsync(wishlistItem);
    }
}
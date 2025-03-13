using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Service.Cart;

public class CartService : ICartService
{
    private readonly ILogger<CartService> _logger;
    private readonly IUserService _userService;
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMemoryCache _cache;
    
    private const string _cartCacheKey = "cart_";
    private const string _cartItemCacheKey = "cartItem_";

    public CartService(ILogger<CartService> logger, IUserService userService, ICartRepository cartRepository, ICartItemRepository cartItemRepository, IMemoryCache cache)
    {
        _logger = logger;
        _userService = userService;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _cache = cache;
    }

    public async Task<List<CartItemDTO>?> GetCartsByRefreshTokenAsync(string token)
    {
        var user = await _userService.FindUserByRefreshToken(token);
        var cartId = await _cartRepository.FindCartByUserId(user.Id)
            .Select(i => i.Id)
            .FirstOrDefaultAsync();

        if (!_cache.TryGetValue(_cartItemCacheKey + "cartItems" + $"{cartId}", out List<CartItemDTO> cachedCartItems))
        {
            var cartItemsQuery = _cartItemRepository.GetCartItemsByCartId(cartId);
        
            cachedCartItems = await cartItemsQuery
                .ProjectToType<CartItemDTO>()
                .ToListAsync();

            if (cachedCartItems.Any())
                _cache.Set(_cartCacheKey + "carts" + $"{cartId}", cachedCartItems,TimeSpan.FromMinutes(5));
            
            foreach (var cartItem in cachedCartItems)
                _cache.Set(_cartItemCacheKey + $"{cartItem.Id}", cartItem,TimeSpan.FromMinutes(5));
        }
        
        return cachedCartItems;
    }

    public async Task<bool> DellCartItemAsync(int cartItemId)
    {
        var cartItem = await _cartItemRepository
            .GetById(cartItemId)
            .FirstOrDefaultAsync();
        
        if (cartItem == null)
            return false;
        
        await _cartItemRepository.DeleteAsync(cartItem);
        
        return true;
    }
    
    public async Task<bool> AddCartItemAsync(ProductIdTokenRequest request)
    {
        var user = await _userService.FindUserByRefreshToken(request.RefreshToken);
        if (user == null)
            return false;
        
        var cart = await _cartRepository.FindCartByUserId(user.Id).FirstOrDefaultAsync();
        if (cart == null)
        {
            cart = new Domain.Entities.Cart
            {
                AppUserId = user.Id,
            };

            await _cartRepository.CreateAsync(cart);
        }
        
        var cartItem = await _cartItemRepository
            .GetCartItemsByCartAndProductId(cart.Id, request.ProductId)
            .FirstOrDefaultAsync();
        if (cartItem == null)
        {
            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = 1
            };
        
            await _cartItemRepository.CreateAsync(cartItem);
        }
        else
        {
            cartItem.Quantity += 1;
        }
        
        await _cartItemRepository.UpdateAsync(cartItem);
        
        return true;
    }

    public async Task<bool> UpdateQuantityAsync(UpdateCartItemRequest request)
    {
        var cartItem = await _cartItemRepository
            .GetById(request.CartItemId)
            .FirstOrDefaultAsync();
        if (cartItem == null)
            return false;
        
        cartItem.Quantity = request.Quantity;
        return await _cartItemRepository.UpdateAsync(cartItem);
    }
}
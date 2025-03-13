using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Application.DTOs.RefreshToken;
using PixelPulse.Application.DTOs.Review;
using PixelPulse.Application.DTOs.Wishlist;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    
    private const string AuthAddress = "http://localhost:5144/api/Auth/";
    private const string CartAddress = "http://localhost:5144/api/Cart/";
    private const string OrderAddress = "http://localhost:5144/api/Order/";
    private const string ProductAddress = "http://localhost:5144/api/Product/";
    private const string ReviewAddress = "http://localhost:5144/api/Review/";
    private const string UserAddress = "http://localhost:5144/api/User/";
    private const string WishlistAddress = "http://localhost:5144/api/Wishlist/";

    public AccountController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    [HttpGet]
    public async Task<IActionResult> Account()
    {
        return View();
    }

    #region user

    [HttpGet]
    public async Task<IActionResult> Personal_Information()
    {
        var response = await _httpClient.PostAsJsonAsync(UserAddress + "personal_Information", new RefreshTokenRequest
        {
            RefreshToken = Request.Cookies["refreshTokenKey"]
        });
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var appUser = JsonSerializer.Deserialize<AppUser>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(appUser);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save_Personal_Information(AppUser appUser)
    {
        var userUpdateRequest = new UserUpdateRequest
        {
            AppUser = appUser,
            RefreshToken = Request.Cookies["refreshTokenKey"]
        };
        var response = await _httpClient.PostAsJsonAsync(UserAddress + "save_personal_Information", userUpdateRequest);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<AppUser>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return RedirectToAction("Personal_Information", "Account", user);
        }
        
        return NoContent();
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshTokenKey"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var response = await _httpClient.PostAsJsonAsync(UserAddress + "logout", refreshToken);
            if (response.IsSuccessStatusCode)
            {
                Response.Cookies.Delete("refreshTokenKey");
            }
        }
        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region order

    [HttpGet]
    public async Task<IActionResult> Order()
    {
        var response = await _httpClient.PostAsJsonAsync(OrderAddress + "orderView", new RefreshTokenRequest
        {
            RefreshToken = Request.Cookies["refreshTokenKey"]
        });
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var orderDtos = JsonSerializer.Deserialize<List<OrderDTO>>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(orderDtos);
        }

        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var response = await _httpClient.GetAsync(OrderAddress + "cancelOrder/" + orderId);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Order");
        }
        
        return NoContent();
    }

    #endregion

    #region cart

    [HttpGet]
    public async Task<IActionResult> Cart()
    {
        var response = await _httpClient.PostAsJsonAsync(CartAddress + "cartView", new RefreshTokenRequest
        {
            RefreshToken = Request.Cookies["refreshTokenKey"]
        });
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var cartItemsDtos = JsonSerializer.Deserialize<List<CartItemDTO>>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(cartItemsDtos);
        }

        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> DeleteCartItem(int cartItemId)
    {
        var response = await _httpClient.GetAsync(CartAddress + "dellCart/" + cartItemId);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Cart");
        }
        
        return NoContent();
    }

    #endregion

    #region wishlist

    [HttpGet]
    public async Task<IActionResult> Wishlist()
    {
        var response = await _httpClient.PostAsJsonAsync(WishlistAddress + "wishlistView", new RefreshTokenRequest
        {
            RefreshToken = Request.Cookies["refreshTokenKey"]
        });
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var wishlistItemDtos = JsonSerializer.Deserialize<List<WishlistItemDTO>>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(wishlistItemDtos);
        }

        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> DeleteWishlistItem(int wishlistItemId)
    {
        var response = await _httpClient.GetAsync(WishlistAddress + "dellWishlist/" + wishlistItemId);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Wishlist");
        }
        
        return NoContent();
    }

    #endregion

    #region review

    [HttpGet]
    public async Task<IActionResult> Review()
    {
        var response = await _httpClient.PostAsJsonAsync(ReviewAddress + "reviewView", new RefreshTokenRequest
        {
            RefreshToken = Request.Cookies["refreshTokenKey"]
        });
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var reviewDtos = JsonSerializer.Deserialize<List<ReviewDTO>>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(reviewDtos);
        }

        return View();
    }

    #endregion
    
}
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Application.DTOs.Review;
using PixelPulse.Application.DTOs.Wishlist;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    
    private const string Address = "http://localhost:5144/api/Account/";

    public AccountController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    [HttpGet]
    public async Task<IActionResult> Account()
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Personal_Information()
    {
        var response = await _httpClient.PostAsJsonAsync(Address + "personal_Information", new Dictionary<string, string>
        {
            {"refreshTokenKey", Request.Cookies["refreshTokenKey"]}
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
        var response = await _httpClient.PostAsJsonAsync(Address + "save_personal_Information", userUpdateRequest);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<AppUser>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(user);
        }
        
        return View(appUser);
    }

    [HttpGet]
    public async Task<IActionResult> Order()
    {
        var response = await _httpClient.PostAsJsonAsync(Address + "order", new Dictionary<string, string>
        {
            {"refreshTokenKey", Request.Cookies["refreshTokenKey"]}
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
    public async Task<IActionResult> Cart()
    {
        var response = await _httpClient.PostAsJsonAsync(Address + "cart", new Dictionary<string, string>
        {
            {"refreshTokenKey", Request.Cookies["refreshTokenKey"]}
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
    public async Task<IActionResult> Wishlist()
    {
        var response = await _httpClient.PostAsJsonAsync(Address + "wishlist", new Dictionary<string, string>
        {
            {"refreshTokenKey", Request.Cookies["refreshTokenKey"]}
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
    public async Task<IActionResult> Review()
    {
        var response = await _httpClient.PostAsJsonAsync(Address + "review", new Dictionary<string, string>
        {
            {"refreshTokenKey", Request.Cookies["refreshTokenKey"]}
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

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshTokenKey"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var response = await _httpClient.PostAsJsonAsync(Address + "logout", refreshToken);
            if (response.IsSuccessStatusCode)
            {
                Response.Cookies.Delete("refreshTokenKey");
            }
        }
        return RedirectToAction("Index", "Home");
    }
    
}
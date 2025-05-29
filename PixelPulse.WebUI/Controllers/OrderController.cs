using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.RefreshToken;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Controllers;

public class OrderController : Controller
{
    private readonly HttpClient _httpClient;
    private const string OrderAddress = "http://localhost:5144/api/Order/";
    private const string UserAddress = "http://localhost:5144/api/User/";
    private const string CartAddress = "http://localhost:5144/api/Cart/";

    public OrderController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var responseCart = await _httpClient.PostAsJsonAsync(CartAddress + "cartView", new RefreshTokenRequest
        {
            RefreshToken = Request.Cookies["refreshTokenKey"]
        });
        var responseUser = await _httpClient.PostAsJsonAsync(UserAddress + "personal_Information", new RefreshTokenRequest
        {
            RefreshToken = Request.Cookies["refreshTokenKey"]
        });
        if (responseCart.IsSuccessStatusCode && responseUser.IsSuccessStatusCode)
        {
            var responseCartString = await responseCart.Content.ReadAsStringAsync();
            var cartItemDtos = JsonSerializer.Deserialize<List<CartItemDTO>>(responseCartString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            var responseUserString = await responseUser.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<AppUser>(responseUserString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var createOrder = new CreateOrderDTO
            {
                CartItemDtos = cartItemDtos,
                AppUserId = user.Id,
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Phonenumber = user.PhoneNumber
            };
            return View(createOrder);
        }
        
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> CheckoutAPI(CreateOrderDTO createOrder)
    {
        var response = await _httpClient.PostAsJsonAsync(OrderAddress + "createOrder", createOrder);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(responseString);
            var shipping = JsonSerializer.Deserialize<Shipping>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return RedirectToAction("SuccessCheckout", shipping);
        }

        return NoContent();
    }
    
    public async Task<IActionResult> SuccessCheckout(Shipping shiping)
    {
        return View(shiping);
    }
}
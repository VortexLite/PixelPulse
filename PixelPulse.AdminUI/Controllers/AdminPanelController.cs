using System.Text.Json;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.DTOs.User;
using PixelPulse.Domain.Entities;

namespace PixelPulse.AdminUI.Controllers;

public class AdminPanelController : Controller
{
    private readonly ILogger<AdminPanelController> _logger;
    private readonly HttpClient _httpClient;
    
    private const string ProductAddress = "http://localhost:5144/api/Product/";
    private const string UserAddress = "http://localhost:5144/api/User/";
    private const string OrderAddress = "http://localhost:5144/api/Order/";

    public AdminPanelController(ILogger<AdminPanelController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region user

    public async Task<IActionResult> UserControl(int page = 1)
    {
        var response = await _httpClient.PostAsJsonAsync(UserAddress + "getusers", page);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<UsersAndCountDTO>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(userDto);
        }

        return NoContent();
    }
    
    public async Task<IActionResult> CreateUser()
    {
        return View();
    }

    public async Task<IActionResult> UpdateUser(string userId)
    {
        var response = await _httpClient.PostAsJsonAsync(UserAddress + "personal_Information", new RefreshRequest
        {
            RefreshToken = Request.Cookies["refreshTokenKey"]
        });
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<AppUser>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(user);
        }
        
        return NoContent();
    }
    
    public async Task<IActionResult> UpdateUserAPI(AppUser user)
    {
        var response = await _httpClient.PostAsJsonAsync(user + "updateUser", new UserUpdateRequest
        {
            AppUser = user,
            RefreshToken = Request.Cookies["refreshTokenKey"]
        });
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("UserControl");
        }
        
        return NoContent();
    }
    
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var response = await _httpClient.GetAsync(ProductAddress + "dellUser/" + userId);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("UserControl");
        }
        
        return NoContent();
    }
    
    
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshTokenKey"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var response = await _httpClient.PostAsJsonAsync(ProductAddress + "logout", refreshToken);
            if (response.IsSuccessStatusCode)
            {
                Response.Cookies.Delete("refreshTokenKey");
            }
        }
        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region product

    public async Task<IActionResult> ProductControl(int page = 1)
    {
        var response = await _httpClient.PostAsJsonAsync(ProductAddress + "GetProductsTake50", page);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<ProductAndCountDTO>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(userDto);
        }

        return NoContent();
    }
    
    public async Task<IActionResult> CreateProduct()
    {
        return View();
    }

    public async Task<IActionResult> UpdateProduct(int productId)
    {
        var response = await _httpClient.GetFromJsonAsync<Product>(ProductAddress + $"getProduct/{productId}");
        return View(response);
    }
    
    public async Task<IActionResult> UpdateProductAPI(ProductUpdateRequest request)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(request.Id.ToString()), "Id");
        content.Add(new StringContent(request.Name), "Name");
        content.Add(new StringContent(request.Description), "Description");
        content.Add(new StringContent(request.OldPrice.ToString()), "OldPrice");
        content.Add(new StringContent(request.Price.ToString()), "Price");
        content.Add(new StringContent(request.Stock.ToString()), "Stock");
        content.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");
        content.Add(new StringContent(request.SupplierId.ToString()), "SupplierId");

        if (request.ProductImages != null)
        {
            foreach (var image in request.ProductImages)
            {
                var stream = new MemoryStream();
                await image.CopyToAsync(stream);
                stream.Position = 0;
                content.Add(new StreamContent(stream), "ProductImages", image.FileName);
            }
        }
        
        var response = await _httpClient.PostAsync(ProductAddress + "updateProduct", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ProductControl");
        }
        
        return NoContent();
    }
    
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var response = await _httpClient.GetAsync(ProductAddress + $"dellProduct/{productId}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ProductControl");
        }
        
        return NoContent();
    }
    
    public async Task<IActionResult> MainImage(ProductImageIdDTO dto)
    {
        var response = await _httpClient.PostAsJsonAsync(ProductAddress + "mainImage", dto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ProductControl");
        }
        
        return NoContent();
    }
    
    public async Task<IActionResult> DeleteImage(int imageId)
    {
        var response = await _httpClient.DeleteAsync(ProductAddress + $"dellImage/{imageId}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ProductControl");
        }
        
        return NoContent();
    }

    #endregion

    #region order

    public async Task<IActionResult> OrderControl(int page = 1)
    {
        var response = await _httpClient.GetFromJsonAsync<OrderAndCountDTO>(OrderAddress + $"getOrderTake50/{page}");
        return View(response);
    }
    
    public async Task<IActionResult> CreateOrder()
    {
        return View();
    }

    public async Task<IActionResult> UpdateOrder(int orderId)
    {
        var response = await _httpClient.GetFromJsonAsync<OrderUpdateDTO>(OrderAddress + $"getOrder/{orderId}");
        return View(response);
    }
    
    public async Task<IActionResult> UpdateOrderAPI(OrderUpdateDTO orderUpdateDto)
    {
        var response = await _httpClient.PostAsJsonAsync(ProductAddress + "updateOrder", orderUpdateDto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ProductControl");
        }
        
        return NoContent();
    }
    
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        var response = await _httpClient.GetAsync(ProductAddress + $"dellProduct/{orderId}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ProductControl");
        }
        
        return NoContent();
    }
    
    public async Task<IActionResult> AddOrderItem()
    {
        return View();
    }
    
    public async Task<IActionResult> DeleteOrderItem()
    {
        return View();
    }


    #endregion
}
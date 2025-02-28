using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly HttpClient _httpClient;
    
    private const string Address = "http://localhost:5144/api/Product/";

    public ProductController(ILogger<ProductController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync(Address + $"GetDetailsProduct/{id}");
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var productDtos = JsonSerializer.Deserialize<ProductCategoryDTO>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            string viewName = productDtos.CategoryName switch
            {
                "Навушники" => "ProductHeadphone",
                "Клавіатура" => "ProductKeyboard",
                "Ноутбук" => "ProductLaptop",
                "Монітор" => "ProductMonitor",
                "Мишка" => "ProductMouse",
                "Смартфон" => "ProductPhone"
            };
            
            return View(viewName, productDtos);
        }
        
        return NoContent();
    }
}
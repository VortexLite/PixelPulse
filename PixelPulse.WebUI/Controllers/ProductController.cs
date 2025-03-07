using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Product;

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
        var response = await _httpClient.GetFromJsonAsync<ProductCategoryDTO>(Address + $"GetDetailsProduct/{id}");
        if (response == null)
            return NoContent();
        
        string viewName = response.CategoryName switch
        {
            "Навушники" => "ProductHeadphone",
            "Клавіатура" => "ProductKeyboard",
            "Ноутбук" => "ProductLaptop",
            "Монітор" => "ProductMonitor",
            "Мишка" => "ProductMouse",
            "Смартфон" => "ProductPhone"
        };
            
        return View(viewName, response);
    }
}
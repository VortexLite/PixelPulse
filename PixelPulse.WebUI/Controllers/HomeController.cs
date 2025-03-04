using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PixelPulse.Application.DTOs.Product;

namespace PixelPulse.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HomeController> _logger;

    private const string Address = "http://localhost:5144/api/Home/";
    public HomeController(HttpClient httpClient, ILogger<HomeController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetStringAsync(Address + "GetProductsForMain");
        var products = JsonConvert.DeserializeObject<List<ProductMainDTO>>(response);
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> AddCart(int id)
    {
        var response = await _httpClient.PostAsJsonAsync(Address + "addCart", new Dictionary<string, string>
        {
            {"refreshTokenKey", Request.Cookies["refreshTokenKey"]},
            {"productId", id.ToString()},
        });

        return NoContent();
    }
}
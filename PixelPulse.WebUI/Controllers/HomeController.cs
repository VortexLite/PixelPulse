using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Product;

namespace PixelPulse.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HomeController> _logger;
    
    private const string CartAddress = "http://localhost:5144/api/Cart/";
    private const string OrderAddress = "http://localhost:5144/api/Order/";
    private const string ProductAddress = "http://localhost:5144/api/Product/";
    private const string ReviewAddress = "http://localhost:5144/api/Home/";
    private const string UserAddress = "http://localhost:5144/api/User/";
    private const string WishlistAddress = "http://localhost:5144/api/Wishlist/";
    
    public HomeController(HttpClient httpClient, ILogger<HomeController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
   public async Task<IActionResult> Index()
    {
        var products = await _httpClient.GetFromJsonAsync<List<ProductMainDTO>>(ProductAddress + "GetProductsForMain");
        var refreshtoken = Request.Cookies["refreshTokenKey"];
        if (refreshtoken != null)
            ViewBag.RefreshToken = refreshtoken;
        
        return View(products);
    }
    
    [HttpGet]
    public async Task<IActionResult> AddCart(int productId)
    {
        var response = await _httpClient.PostAsJsonAsync(CartAddress + "addCart", new ProductIdTokenRequest
        {
            RefreshToken = Request.Cookies["refreshTokenKey"],
            ProductId = productId
        });

        return NoContent();
    }
    
    [HttpGet]
    public async Task<IActionResult> AddWishlist(int productId)
    {
        await _httpClient.PostAsJsonAsync(WishlistAddress + "addWishlist", new ProductIdTokenRequest
        {
            RefreshToken = Request.Cookies["refreshTokenKey"],
            ProductId = productId
        });

        return NoContent();
    }
}
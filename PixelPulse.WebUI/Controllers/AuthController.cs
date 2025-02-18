using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Account;

namespace PixelPulse.Controllers;

[Route("[controller]")]
public class AuthController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthController> _logger;
    
    private const string Address = "http://localhost:5144/api/Auth/";

    public AuthController(HttpClient httpClient, ILogger<AuthController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    [HttpGet("login")]
    public async Task<IActionResult> LoginAsync()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody]Login model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        var result = await _httpClient.PostAsJsonAsync(Address + "login", model);
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
            return RedirectToAction("Index", "Home");
        
        ModelState.AddModelError(string.Empty, "Invalid login attempt");
        return View(model);
    }

    [HttpGet("register")]
    public async Task<IActionResult> RegisterAsync()
    {
        return View();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(SignUp model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        var result = await _httpClient.PostAsJsonAsync(Address + "signUp", model);
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
            return RedirectToAction("Index", "Home");
        
        ModelState.AddModelError(string.Empty, "Invalid register attempt");
        return View(model);
    }
}
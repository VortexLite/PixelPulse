using System.Net;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Account;
using System.Text.Json;


namespace PixelPulse.Controllers;

[Route("[controller]")]
public class AuthController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthController> _logger;
    private readonly CookieContainer _cookieContainer;
    
    private const string Address = "http://localhost:5144/api/Auth/";

    public AuthController(HttpClient httpClient, ILogger<AuthController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _cookieContainer = new CookieContainer();
        var handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = _cookieContainer,
            AllowAutoRedirect = true
        };
        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(Address)
        };
    }
    
    private async Task<AuthResponse?> TryAutoLoginAsync()
    {
        if (Request.Cookies.TryGetValue("refreshTokenKey", out var refreshToken))
        {
            var response = await _httpClient.PostAsJsonAsync(Address + "refreshTokenCheck", new Dictionary<string, string>
            {
                {"refreshTokenKey", refreshToken}
            });

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (authResponse != null && authResponse.IsAuthenticated)
                {
                    Response.Cookies.Delete("refreshTokenKey");
                    Response.Cookies.Append("refreshTokenKey", authResponse.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = authResponse.RefreshTokenExpiration
                    });

                    return authResponse;
                }
            }
        }
        
        return null;
    }
    
    [HttpGet("login")]
    public async Task<IActionResult> LoginAsync()
    {
        var authResponse = await TryAutoLoginAsync();
        if (authResponse != null && authResponse.IsAuthenticated)
            return RedirectToAction("Index", "Home");
        
        return View();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(Login model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        var response = await _httpClient.PostAsJsonAsync(Address + "login", model);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            if (authResponse?.Roles is null)
            {
                var jsonDoc = JsonDocument.Parse(responseString);
                if (jsonDoc.RootElement.TryGetProperty("roles", out var rolesElement) && rolesElement.ValueKind == JsonValueKind.String)
                {
                    authResponse.Roles = rolesElement.GetString()?.Split(',').ToList() ?? new List<string>();
                }
            }
            
            Response.Cookies.Delete("refreshTokenKey");
            Response.Cookies.Append("refreshTokenKey", authResponse.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = authResponse.TokenExpiresOn
            });
            
            return RedirectToAction("Index", "Home");
        }
        
        ModelState.AddModelError(string.Empty, "Invalid login attempt");
        return View(model);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        /*Response.Cookies.Delete("refreshTokenKey");
        var response = await _httpClient.PostAsync(Address + "revokeToken", null);
        if (response.IsSuccessStatusCode)
            return RedirectToAction("Index", "Home");
            
        ModelState.AddModelError("", "Не вдалося вийти з системи");
        return RedirectToAction("Index", "Home");*/
        var refreshToken = Request.Cookies["refreshTokenKey"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var response = await _httpClient.PostAsJsonAsync(Address + "revokeToken", refreshToken);
            if (response.IsSuccessStatusCode)
            {
                Response.Cookies.Delete("refreshTokenKey");
                return RedirectToAction("Index", "Home");
            }
        }
        
        return RedirectToAction("Index", "Home");
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
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Account;
using PixelPulse.Application.Interfaces.Authentication;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthResponse _authService;

    public AuthController(IAuthResponse authResponse)
    {
        _authService = authResponse;
    }

    private void SetRefreshTokenInCookies(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime()
        };
        
        Response.Cookies.Append("refreshTokenKey", refreshToken, cookieOptions);
    }

    [HttpPost("signUp")]
    public async Task<IActionResult> SignUpAsync([FromBody] SignUp model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var origin = Request.Headers["origin"];
        var result = await _authService.SignUpAsync(model, origin);
        if (!result.IsAuthenticated)
            return BadRequest(result.Message);
        
        SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);
        
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(Login model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _authService.LoginAsync(model);
        if (!result.IsAuthenticated)
            return BadRequest(result.Message);
        
        if (!string.IsNullOrEmpty(result.RefreshToken))
            SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);
        
        return Ok(result);
    }

    [HttpPost("AddRole")]
    public async Task<IActionResult> AddRoleAsync(AssignRolesDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _authService.AssignRolesAsync(model);
        if (!string.IsNullOrEmpty(result))
            return BadRequest(result);
        
        return Ok(result);
    }

    [HttpGet("refreshToken")]
    public async Task<IActionResult> RefreshTokenCheckAsync()
    {
        var refreshToken = Request.Cookies["refreshTokenKey"];
        var result = await _authService.RefreshTokenCheckAsync(refreshToken);
        if (!result.IsAuthenticated)
            return BadRequest(result);
        
        SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);
        return Ok(result);
    }

    [HttpPost("revokeToken")]
    public async Task<IActionResult> RevokeTokenAsync(RevokeToken model)
    {
        var refreshToken = model.Token ?? Request.Cookies["refreshTokenKey"];
        
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest("Token is required");
        
        var result = await _authService.RevokeTokenAsync(refreshToken);
        
        return Ok("Done revoke");
    }
}
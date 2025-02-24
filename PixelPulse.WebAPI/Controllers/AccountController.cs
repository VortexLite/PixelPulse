using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Account;
using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Application.Interfaces.Authentication;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;
    private readonly AppIdentityContext _context;
    private readonly IAuthResponse _authService;
    

    public AccountController(ILogger<AccountController> logger, AppIdentityContext context, IAuthResponse authService, IAccountService accountService)
    {
        _logger = logger;
        _context = context;
        _authService = authService;
        _accountService = accountService;
    }

    [HttpPost("personal_Information")]
    public async Task<IActionResult> PersonalInformation([FromBody] Dictionary<string, string> refreshToken)
    {
        var user = await _accountService.FindUserByRefreshToken(refreshToken.GetValueOrDefault("refreshTokenKey"));
        if (user == null)
            return Unauthorized("Invalid refresh token");
        
        return Ok(user);
    }
    
    [HttpPost("save_personal_Information")]
    public async Task<IActionResult> Save_PersonalInformation([FromBody] UserUpdateRequest request)
    {
        var user = await _accountService.UpdateAccount(request);
        if (user == null)
            BadRequest("Invalid request");
        
        return Ok(user);
    }

    [HttpPost("order")]
    public async Task<IActionResult> Order([FromBody] Dictionary<string, string> refreshToken)
    {
        var orderDtos = await _accountService.GetOrdersByRefreshToken(refreshToken.GetValueOrDefault("refreshTokenKey"));
        
        return Ok(orderDtos);
    }
    
    [HttpPost("cart")]
    public async Task<IActionResult> Cart([FromBody] Dictionary<string, string> refreshToken)
    {
        var cartDtos = await _accountService.GetCartsByRefreshToken(refreshToken.GetValueOrDefault("refreshTokenKey"));
        
        return Ok(cartDtos);
    }
    
    [HttpPost("wishlist")]
    public async Task<IActionResult> Wishlist([FromBody] Dictionary<string, string> refreshToken)
    {
        var wishlistDtos = await _accountService.GetWishlistByRefreshToken(refreshToken.GetValueOrDefault("refreshTokenKey"));
        
        return Ok(wishlistDtos);
    }
    
    [HttpPost("review")]
    public async Task<IActionResult> Review([FromBody] Dictionary<string, string> refreshToken)
    {
        var reviewDtos = await _accountService.GetReviewByRefreshToken(refreshToken.GetValueOrDefault("refreshTokenKey"));
        
        return Ok(reviewDtos);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync(RevokeToken model)
    {
        var refreshToken = model.Token ?? Request.Cookies["refreshTokenKey"];
        
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest("Token is required");
        
        var result = await _authService.RevokeTokenAsync(refreshToken);
        Response.Cookies.Delete("refreshTokenKey");
        
        return Ok("Done logout");
    }
}
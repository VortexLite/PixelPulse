using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.DTOs.RefreshToken;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WishlistController : ControllerBase
{
    private readonly ILogger<WishlistController> _logger;
    private readonly IWishlistService _wishlistService;

    public WishlistController(ILogger<WishlistController> logger, IWishlistService wishlistService)
    {
        _logger = logger;
        _wishlistService = wishlistService;
    }
    
    [HttpPost("wishlistView")]
    public async Task<IActionResult> Wishlist([FromBody] RefreshTokenRequest refreshToken)
    {
        return Ok(await _wishlistService.GetWishlistByRefreshTokenAsync(refreshToken.RefreshToken));
    }
    
    [HttpPost("addWishlist")]
    public async Task<IActionResult> AddWishlist([FromBody] ProductIdTokenRequest request)
    {
        if (await _wishlistService.AddWishlistItemAsync(request))
        {
            return Ok();
        }
        
        return BadRequest();
    }
    
    [HttpGet("dellWishlist/{wishlistItemId}")]
    public async Task<IActionResult> DellWishlistItem(int wishlistItemId)
    {
        if (await _wishlistService.RemoveWishlistItemAsync(wishlistItemId))
        {
            return Ok();
        }
        return BadRequest();
    }
}
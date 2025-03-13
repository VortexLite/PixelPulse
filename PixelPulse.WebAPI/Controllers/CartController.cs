using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.DTOs.RefreshToken;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ILogger<CartController> _logger;
    private readonly ICartService _cartService;

    public CartController(ILogger<CartController> logger, ICartService cartService)
    {
        _logger = logger;
        _cartService = cartService;
    }
    
    [HttpPost("cartView")]
    public async Task<IActionResult> Cart([FromBody] RefreshTokenRequest refreshToken)
    {
        var cartDtos = await _cartService.GetCartsByRefreshTokenAsync(refreshToken.RefreshToken);
        
        return Ok(cartDtos);
    }

    [HttpPost("addCart")]
    public async Task<IActionResult> AddCart([FromBody] ProductIdTokenRequest request)
    {
        if (await _cartService.AddCartItemAsync(request))
        {
            return Ok();
        }
        
        return BadRequest();
    }
    
    [HttpGet("dellCart/{cartItemId}")]
    public async Task<IActionResult> DellCart(int cartItemId)
    {
        if (await _cartService.DellCartItemAsync(cartItemId))
        {
            return Ok();
        }
        return BadRequest();
    }
    
    [HttpPost("updateCart")]
    public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemRequest request)
    {
        if (await _cartService.UpdateQuantityAsync(request))
        {
            return Ok();
        }
        
        return BadRequest();
    }
}
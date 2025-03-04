using Mapster;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IHomeService homeService, ILogger<HomeController> logger)
    {
        _homeService = homeService;
        _logger = logger;
    }
    
    [HttpGet("GetProductsForMain")]
    public async Task<IActionResult> GetProductsForMain()
    {
        var productDTO = await _homeService.GetAllProductWithImageTake20Async();
        if (productDTO is null)
            return NotFound();
        
        _logger.LogInformation($"Converting productMainDTO {productDTO.Count} products");
        return Ok(productDTO);
    }

    [HttpPost("addCart")]
    public async Task<IActionResult> AddCart([FromBody] Dictionary<string, string> productAndToken)
    {
        var acceptCartItem = await _homeService.AddCartItemAsync(productAndToken);
        return Ok(acceptCartItem);
    }
}
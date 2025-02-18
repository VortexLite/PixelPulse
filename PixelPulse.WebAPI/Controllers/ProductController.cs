using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.Interfaces.Repositories;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }
    
    [HttpGet("GetProductsForMain")]
    public async Task<IActionResult> GetProductsForMain()
    {
        var products = await _productRepository.GetAllProductWithImageTake20Async();
        if (products is null) 
            return NotFound();
        
        var productDTO = products.Adapt<List<ProductMainDTO>>();
        _logger.LogInformation($"Converting productMainDTO {productDTO.Count} products");
        return Ok(productDTO);
    }
}
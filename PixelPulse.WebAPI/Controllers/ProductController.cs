using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;


    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet("GetProductsForMain")]
    public async Task<IActionResult> GetProductsForMain()
    {
        var productsDtos = await _productService.GetProductsWithImageTake20Async();
        if (productsDtos == null)
            return NotFound();
        
        return Ok(productsDtos);
    }

    [HttpGet("GetDetailsProduct/{id}")]
    public async Task<IActionResult> GetDetailsProduct(int id)
    {
        var productDtos = await _productService.GetDetailsProduct(id);
        if (productDtos == null)
            return NotFound();
        
        return Ok (productDtos);
    }

}
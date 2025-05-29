using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Filter;
using PixelPulse.Application.DTOs.Product;
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
    
    [HttpGet("GetProductsTake100")]
    public async Task<IActionResult> GetProductsTake100()
    {
        var productsDtos = await _productService.GetProductsWithImageTake20Async();
        if (productsDtos == null)
            return NotFound();
        
        return Ok(productsDtos);
    }
    
    [HttpPost("GetFilteredProduct")]
    public async Task<IActionResult> GetFilteredProduct([FromBody] FilterPhoneDTO filter)
    {
        var productsDtos = await _productService.GetFilteredProduct(filter);
        if (productsDtos == null)
            return NotFound();
        
        return Ok(productsDtos);
    }
    
    [HttpPost("GetProductsTake50")]
    public async Task<IActionResult> GetProductsTake50([FromBody] int page)
    {
        var productsDtos = await _productService.GetProductTake50(page);
        if (productsDtos == null)
            return NotFound();
        
        return Ok(productsDtos);
    }
    
    [HttpGet("getProduct/{productId}")]
    public async Task<IActionResult> GetProductById(int productId)
    {
        var product = await _productService.GetProducWithImageById(productId);
        if (product != null)
        {
            return Ok(product);
        }
        
        return BadRequest("Product not found");
    }
    
    [HttpPost("updateProduct")]
    public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateRequest request)
    {
        if (await _productService.UpdateProductWithImage(request))
        {
            return Ok();
        }
        
        return BadRequest("Failed to update product");
    }
    
    [HttpGet("dellProduct/{productId}")]
    public async Task<IActionResult> DellProduct([FromBody] int productId)
    {
        if (await _productService.DeleteProduct(productId))
        {
            return Ok();
        }
        
        return BadRequest();
    }
    
    [HttpPost("mainImage")]
    public async Task<IActionResult> MainImage([FromBody] ProductImageIdDTO dto)
    {
        if (await _productService.MainProductImage(dto))
        {
            return Ok();
        }
        
        return BadRequest();
    }
    
    [HttpDelete("dellImage/{imageId}")]
    public async Task<IActionResult> DellImage(int imageId)
    {
        if (await _productService.DeleteProductImage(imageId))
        {
            return Ok();
        }
        
        return BadRequest();
    }
    
    [HttpPost("createProduct")]
    public async Task<IActionResult> CreateProduct([FromForm] ProductUpdateRequest request)
    {
        if (await _productService.CreateProductWithImage(request))
        {
            return Ok();
        }
        
        return BadRequest("Failed to create product");
    }
}
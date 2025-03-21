using Microsoft.AspNetCore.Http;

namespace PixelPulse.Application.DTOs.Product;

public class ProductUpdateRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal OldPrice { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }

    public List<IFormFile> ProductImages { get; set; }
}
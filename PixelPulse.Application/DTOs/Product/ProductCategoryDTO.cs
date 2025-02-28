using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.DTOs.Product;

public class ProductCategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string CategoryName { get; set; }
    public List<ProductImage> ProductImages { get; set; }
    public List<ProductSpecificationDTO> ProductSpecifications { get; set; }
}
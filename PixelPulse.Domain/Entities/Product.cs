namespace PixelPulse.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal OldPrice { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public Review? Review { get; set; }
    public WishlistItem? WishlistItem { get; set; }
    
    public OrderItem? OrderItem { get; set; }
    public List<ProductImage>? ProductImages { get; set; }
    public List<ProductSpecification>? ProductSpecifications { get; set; }
}
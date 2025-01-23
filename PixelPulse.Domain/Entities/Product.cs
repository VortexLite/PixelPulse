namespace PixelPulse.Domain.Entities;

public class Product
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual ICollection<Review>? Reviews { get; set; }
    public virtual ICollection<ProductImage>? ProductImages { get; set; }
    public virtual ICollection<ProductSpecification>? ProductSpecifications { get; set; }
}
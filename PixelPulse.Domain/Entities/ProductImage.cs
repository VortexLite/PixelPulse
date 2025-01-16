namespace PixelPulse.Domain.Entities;

public class ProductImage
{
    public Guid ProductImageId { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public string ImageUrl { get; set; }
    public bool IsMain { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
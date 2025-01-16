namespace PixelPulse.Domain.Entities;

public class ProductSpecification
{
    public Guid ProductSpecificationId { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public string SpecificationName { get; set; }
    public string SpecificationValue { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
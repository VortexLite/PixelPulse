namespace PixelPulse.Domain.Entities;

public class ProductBundle
{
    public Guid ProductBundleId { get; set; }
    public string BundleName { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<ProductBundleItem>? ProductBundleItems { get; set; }
}
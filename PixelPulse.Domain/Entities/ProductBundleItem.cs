namespace PixelPulse.Domain.Entities;

public class ProductBundleItem
{
    public Guid ProductBundleItemId { get; set; }
    public Guid BundleId { get; set; }
    public virtual ProductBundle Bundle { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
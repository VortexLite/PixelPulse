namespace PixelPulse.Domain.Entities;

public class WishlistItem
{
    public Guid WishlistItemId { get; set; }
    public Guid WishlistId { get; set; }
    public virtual Wishlist Wishlist { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
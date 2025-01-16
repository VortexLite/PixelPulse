using System.Collections;

namespace PixelPulse.Domain.Entities;

public class Wishlist
{
    public Guid WishlistId { get; set; }
    public string AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<WishlistItem> WishlistItems { get; set; }
}
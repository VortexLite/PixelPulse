using System.Collections;

namespace PixelPulse.Domain.Entities;

public class Cart
{
    public Guid CartId { get; set; }
    public string AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<CartItem> CartItems { get; set; }
}
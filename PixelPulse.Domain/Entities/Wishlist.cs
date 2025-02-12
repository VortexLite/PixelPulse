namespace PixelPulse.Domain.Entities;

public class Wishlist
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public virtual AppUser User { get; set; }
    public virtual ICollection<WishlistItem>? WishlistItems { get; set; }
}
namespace PixelPulse.Domain.Entities;

public class Wishlist
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public List<WishlistItem>? WishlistItems { get; set; }
}
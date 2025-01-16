using Microsoft.AspNetCore.Identity;

namespace PixelPulse.Domain.Entities;

public class AppUser : IdentityUser 
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string? Position { get; set; }
    public decimal? Salary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual ICollection<Cart> Carts { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<Review> Reviews { get; set; }
    public virtual ICollection<Wishlist> Wishlists { get; set; }
    
}
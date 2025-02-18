using Microsoft.AspNetCore.Identity;
using PixelPulse.Domain.Settings;

namespace PixelPulse.Domain.Entities;

public class AppUser : IdentityUser 
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Address? Address { get; set; }
    public Cart? Cart { get; set; }
    public List<Review>? Reviews { get; set; }
    public Wishlist? Wishlist { get; set; }
    public List<Order>? Orders { get; set; }

    public List<RefreshToken>? RefreshTokens { get; set; }
    
}
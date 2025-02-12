using Microsoft.AspNetCore.Identity;
using PixelPulse.Domain.Settings;

namespace PixelPulse.Domain.Entities;

public class AppUser : IdentityUser 
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    
    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
    public virtual ICollection<Order>? Orders { get; set; }
}
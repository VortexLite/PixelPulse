using Microsoft.AspNetCore.Identity;

namespace PixelPulse.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
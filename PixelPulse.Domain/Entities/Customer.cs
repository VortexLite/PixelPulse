namespace PixelPulse.Domain.Entities;

public class Customer : BaseEntity
{
    public string AppUserId { get; set; }
    public AppUser User { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Adrress { get; set; }
    public int LoyaltyPoints { get; set; }
}
namespace PixelPulse.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public virtual AppUser User  { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    
}
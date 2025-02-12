namespace PixelPulse.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public virtual AppUser  User { get; set; }
    public virtual ICollection<CartItem>? CartItems { get; set; }
}
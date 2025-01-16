namespace PixelPulse.Domain.Entities;

public class CartItem
{
    public Guid CartItemId { get; set; }
    public Guid CartId { get; set; }
    public virtual Cart Cart { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
namespace PixelPulse.Domain.Entities;

public class ReturnItem
{
    public Guid ReturnItemId { get; set; }
    public Guid ReturnId { get; set; }
    public virtual Return Return { get; set; }
    public Guid OrderItemId { get; set; }
    public virtual OrderItem OrderItem { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
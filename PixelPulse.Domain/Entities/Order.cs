namespace PixelPulse.Domain.Entities;

public class Order
{
    public Guid OrderId { get; set; }
    public string AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string OrderStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<OrderItem> OrderItems { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
    public virtual Return Return { get; set; }
}
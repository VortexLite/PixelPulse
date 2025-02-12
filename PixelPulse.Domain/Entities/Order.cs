namespace PixelPulse.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public virtual AppUser User { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}
namespace PixelPulse.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    public Payment? Payment { get; set; }
    public Shipping? Shipping { get; set; }
    public List<OrderItem>? OrderItems { get; set; }
}
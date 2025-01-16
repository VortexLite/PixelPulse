namespace PixelPulse.Domain.Entities;

public class ShippingMethod
{
    public Guid ShippingMethodId { get; set; }
    public string MethodName { get; set; }
    public decimal Cost { get; set; }
    public string EstimatedDeliveryTime { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    //public virtual OrderItem OrderItem { get; set; }
}
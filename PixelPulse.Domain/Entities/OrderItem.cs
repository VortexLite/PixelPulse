namespace PixelPulse.Domain.Entities;

public class OrderItem
{
    public Guid OrderItemId { get; set; }
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public Guid ShippingAddressId { get; set; }
    public virtual ShippingAddress ShippingAddress { get; set; }
    public Guid ShippingMethodId { get; set; }
    public virtual ShippingMethod ShippingMethod { get; set; }
    public string OrderItemStatus { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
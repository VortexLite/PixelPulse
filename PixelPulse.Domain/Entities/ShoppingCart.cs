namespace PixelPulse.Domain.Entities;

public class ShoppingCart : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}
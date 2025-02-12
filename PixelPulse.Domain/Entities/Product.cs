namespace PixelPulse.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal OldPrice { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public virtual ICollection<OrderItem>? OrderItems { get; set; }
    public virtual ICollection<ProductImage>? ProductImages { get; set; }
}
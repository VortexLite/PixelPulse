namespace PixelPulse.Domain.Entities;

public class ProductSpecification
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string? Category { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
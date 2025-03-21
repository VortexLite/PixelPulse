namespace PixelPulse.Application.DTOs.Product;

public class ProductMainDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal OldPrice { get; set; }
    public decimal Price { get; set; }
    public string? ImagePath { get; set; }
}
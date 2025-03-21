namespace PixelPulse.Application.DTOs.Product;

public class ProductAndCountDTO
{
    public List<Domain.Entities.Product> Products { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}
namespace PixelPulse.Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public bool isMain { get; set; }
    public byte[] ImageData { get; set; }
    public string ImageType { get; set; }
}
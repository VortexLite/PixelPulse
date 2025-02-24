namespace PixelPulse.Application.DTOs.Wishlist;

public class WishlistItemDTO
{
    public int Id { get; set; }
    public int WishlistId { get; set; }
    public string ImageType { get; set; }
    public byte[] ImageData { get; set; }
    public string ProductName { get; set; }
    public decimal OldPrice { get; set; }
    public decimal Price { get; set; }
}
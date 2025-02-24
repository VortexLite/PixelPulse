namespace PixelPulse.Application.DTOs.Review;

public class ReviewDTO
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ImageType { get; set; }
    public byte[] ImageData { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
}
namespace PixelPulse.Application.DTOs.Review;

public class ReviewDTO
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string? ImagePath { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
}
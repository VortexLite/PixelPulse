namespace PixelPulse.Domain.Entities;

public class Review
{
    public Guid ReviewId { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public string AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
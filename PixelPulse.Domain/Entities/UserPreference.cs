namespace PixelPulse.Domain.Entities;

public class UserPreference
{
    public Guid UserPreferenceId { get; set; }
    public string UserId { get; set; }
    public virtual AppUser User { get; set; }
    public string PreferenceKey { get; set; }
    public string PreferenceValue { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
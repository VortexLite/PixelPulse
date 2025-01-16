namespace PixelPulse.Domain.Entities;

public class Setting
{
    public Guid SettingId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
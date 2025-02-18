using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Domain.Settings;

[Owned]
public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpireOn { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpireOn;
    public DateTime CreatedOn { get; set; }
    public DateTime? RevokedOn { get; set; }
    public bool isActive => RevokedOn == null && !IsExpired;
}
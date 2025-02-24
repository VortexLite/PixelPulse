using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.DTOs.PersonalInformation;

public class UserUpdateRequest
{
    public AppUser AppUser { get; set; }
    public string RefreshToken { get; set; }
}
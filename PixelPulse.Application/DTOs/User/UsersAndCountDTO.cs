using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.DTOs.User;

public class UsersAndCountDTO
{
    public List<AppUser> AppUsers { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}
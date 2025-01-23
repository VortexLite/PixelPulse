using System.ComponentModel.DataAnnotations;

namespace PixelPulse.Application.DTOs.Account;

public class AssignRolesDTO
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Role { get; set; }
}
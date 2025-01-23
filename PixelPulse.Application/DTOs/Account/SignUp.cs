using System.ComponentModel.DataAnnotations;

namespace PixelPulse.Application.DTOs.Account;

public class SignUp
{
    [Required]
    public string Firstname { get; set; }
    
    [Required]
    public string Lastname { get; set; }
    
    public string Username { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}
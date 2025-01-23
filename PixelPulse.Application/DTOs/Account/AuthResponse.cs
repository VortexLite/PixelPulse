using System.Text.Json.Serialization;

namespace PixelPulse.Application.DTOs.Account;

public class AuthResponse
{
    public string? Messege { get; set; }

    public bool IsAuthenticated { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenExpiresOn { get; set; }

    [JsonIgnore]
    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiration { get; set; } 
}
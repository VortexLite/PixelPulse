using System.Text.Json.Serialization;

namespace PixelPulse.Application.DTOs.Account;

public class AuthResponse
{
    public string? Message { get; set; }

    public bool IsAuthenticated { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    
    [JsonPropertyName("roles")]
    public List<string>? Roles { get; set; } = new();
    public string? Token { get; set; }
    public DateTime? TokenExpiresOn { get; set; }
    
    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiration { get; set; } 
}
using PixelPulse.Application.DTOs.Account;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Authentication;

public interface IAuthResponse
{
    Task<AuthResponse> SignUpAsync(SignUp model, string orgin);
    Task<AuthResponse> LoginAsync(Login model);
    Task<string> AssignRolesAsync(AssignRolesDTO model);
    Task<AuthResponse> RefreshTokenCheckAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
    
}
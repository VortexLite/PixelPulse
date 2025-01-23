using System.IdentityModel.Tokens.Jwt;
using PixelPulse.Application.DTOs.Account;
using PixelPulse.Domain.Entities;
using PixelPulse.Domain.Settings;

namespace PixelPulse.Application.Interfaces.Authentication;

public interface IJWT
{
    Task<JwtSecurityToken> CreateJwtAsync(AppUser user);
    Task<RefreshToken> GenerateRefreshToken();
    
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PixelPulse.Application.DTOs.Account;
using PixelPulse.Application.Interfaces.Authentication;
using PixelPulse.Domain.Entities;
using PixelPulse.Domain.Settings;

namespace PixelPulse.Infrastructure.Services.Auth;

public class AuthResponseService : IAuthResponse, IJWT
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JWT _jwt;

    public AuthResponseService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwt = jwt.Value;
    }


    public async Task<JwtSecurityToken> CreateJwtAsync(AppUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim(ClaimTypes.Role, role));
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("userId", user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwt.DurationInDays),
            signingCredentials: signingCredentials);
        
        return jwtSecurityToken;
    }

    public async Task<RefreshToken> GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var generator = new RNGCryptoServiceProvider();
        
        generator.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpireOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow
        };
    }

    public async Task<AuthResponse> SignUpAsync(SignUp model, string orgin)
    {
        var auth = new AuthResponse();
        
        var userEmail = await _userManager.FindByEmailAsync(model.Email);
        var username = await _userManager.FindByNameAsync(model.Username);
        
        if (userEmail != null)
            return new AuthResponse { Messege = "Email is already used" };
        
        if (username != null)
            return new AuthResponse { Messege = "Username is already used" };

        var user = new AppUser
        {
            FirstName = model.Firstname,
            LastName = model.Lastname,
            UserName = model.Username,
            Email = model.Email
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Empty;
            foreach (var error in result.Errors)
            {
                errors += $"{error.Code}: {error.Description}\n";
            }
            
            return new AuthResponse { Messege = errors };
        }
        
        await _userManager.AddToRoleAsync(user, "User");

        var jwtSecurityKey = await CreateJwtAsync(user);
        
        auth.Email = user.Email;
        auth.Roles = new List<string> { "User" };
        auth.IsAuthenticated = true;
        auth.Username = user.UserName;
        auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityKey);
        auth.TokenExpiresOn = jwtSecurityKey.ValidTo.ToLocalTime();
        auth.Messege = "SignUp Succeeded";
        
        var newRefreshToken = await GenerateRefreshToken();
        auth.RefreshToken = newRefreshToken.Token;
        auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;
        
        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);

        return auth;
    }

    public async Task<AuthResponse> LoginAsync(Login model)
    {
        var auth = new AuthResponse();
        
        var user = await _userManager.FindByEmailAsync(model.Email);
        var userPass = await _userManager.CheckPasswordAsync(user, model.Password);
        if (user == null || !userPass)
        {
            auth.Messege = "Email or Password is incorrect";
            return auth;
        }
        
        var jwtSecurityKey = await CreateJwtAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        
        auth.Email = user.Email;
        auth.Roles = roles.ToList();
        auth.IsAuthenticated = true;
        auth.Username = user.UserName;
        auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityKey);
        auth.TokenExpiresOn = jwtSecurityKey.ValidTo;
        auth.Messege = "Login Succeeded";

        if (user.RefreshTokens.Any(t => t.isActive))
        {
            var activeRefreshTokens = user.RefreshTokens
                .FirstOrDefault(t => t.isActive);
            auth.RefreshToken = activeRefreshTokens.Token;
            auth.RefreshTokenExpiration = activeRefreshTokens.ExpireOn;
        }
        else
        {
            var newRefreshToken = await GenerateRefreshToken();
            auth.RefreshToken = newRefreshToken.Token;
            auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;
            
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
        }
        
        return auth;
    }

    public async Task<string> AssignRolesAsync(AssignRolesDTO model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        var role = await _roleManager.RoleExistsAsync(model.Role);

        if (user == null || !role)
            return "Invalid ID or Role";

        if (await _userManager.IsInRoleAsync(user, model.Role))
            return "User already assigned to this role";
        
        var result = await _userManager.AddToRoleAsync(user, model.Role);
        if (!result.Succeeded)
            return "Something went wrong";

        return string.Empty;
    }

    public async Task<AuthResponse> RefreshTokenCheckAsync(string token)
    {
        var auth = new AuthResponse();
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshTokens
                    .Any(t => t.Token == token));

        if (user == null)
        {
            auth.Messege = "Invalid Token";
            return auth;
        }
        
        var refreshToken = user.RefreshTokens
            .Single(t => t.Token == token);

        if (!refreshToken.isActive)
        {
            auth.Messege = "Inactive Token";
            return auth;
        }
        
        refreshToken.RevokedOn = DateTime.UtcNow;
        var newRefreshToken = await GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);
        
        var jwtSecurityKey = await CreateJwtAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        
        auth.Email = user.Email;
        auth.Roles = roles.ToList();
        auth.IsAuthenticated = true;
        auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityKey);
        auth.TokenExpiresOn = jwtSecurityKey.ValidTo;
        auth.RefreshToken = newRefreshToken.Token;
        auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;
        
        return auth;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshTokens
                .Any(t => t.Token == token));
        if (user == null)
            return false;
        
        var refreshToken = user.RefreshTokens
            .Single(t => t.Token == token);
        if (!refreshToken.isActive)
            return false;
        
        refreshToken.RevokedOn = DateTime.UtcNow;
        var newRefreshToken = await GenerateRefreshToken();
        await _userManager.UpdateAsync(user);
        
        return true;
    }
}
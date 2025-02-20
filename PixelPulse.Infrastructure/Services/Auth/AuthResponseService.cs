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
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Auth;

public class AuthResponseService(
    AppIdentityContext context,
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<JWT> jwt)
    : IAuthResponse, IJWT
{
    private readonly AppIdentityContext _context = context;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly JWT _jwt = jwt.Value;

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
            new Claim("UserId", user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);
        
        return jwtSecurityToken;
    }

    public async Task<RefreshToken> GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        /*using var generator = new RNGCryptoServiceProvider();
        
        generator.GetBytes(randomNumber);*/
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('='),
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
            return new AuthResponse { Message = "Email is already used" };
        
        if (username != null)
            return new AuthResponse { Message = "Username is already used" };

        var user = new AppUser
        {
            UserName = model.Username,
            Email = model.Email,
            RefreshTokens = new List<RefreshToken>()
        };

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Code}: {error.Description}\n";
                }
            
                return new AuthResponse { Message = errors };
            }
        
            await _userManager.AddToRoleAsync(user, "Customer");

            var jwtSecurityKey = await CreateJwtAsync(user);
        
            auth.Email = user.Email;
            auth.Roles = new List<string> { "Customer" };
            auth.IsAuthenticated = true;
            auth.Username = user.UserName;
            auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityKey);
            auth.TokenExpiresOn = jwtSecurityKey.ValidTo.ToLocalTime();
            auth.Message = "SignUp Succeeded";
        
            var newRefreshToken = await GenerateRefreshToken();
            auth.RefreshToken = newRefreshToken.Token;
            auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;
        
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            await transaction.CommitAsync();

            return auth;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new AuthResponse { Message = ex.Message };
        }
    }

    public async Task<AuthResponse> LoginAsync(Login model)
    {
        var auth = new AuthResponse();
        
        var user = await _userManager.FindByEmailAsync(model.Email);
        var userpass = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!userpass)
        {
            auth.Message = "Email or Password is incorrect";
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
        auth.Message = "Login Succeeded";

        if (user.RefreshTokens != null && user.RefreshTokens.Any(t => t.isActive))
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
            
            if (user.RefreshTokens == null)
                user.RefreshTokens = new List<RefreshToken>();
            
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
        }
        
        return auth;
    }

    public async Task<string> AssignRolesAsync(AssignRolesDTO model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        var role = await _roleManager.RoleExistsAsync(model.Role);

        if (user == null || !role || string.IsNullOrWhiteSpace(model.Role))
            return "Invalid ID or Role";

        if (await _userManager.IsInRoleAsync(user, model.Role))
            return "User already assigned to this role";
        
        var result = await _userManager.AddToRoleAsync(user, model.Role);
        if (!result.Succeeded)
            return "Something went wrong";

        return string.Empty;
    }

    /*public async Task<AuthResponse> RefreshTokenCheckAsync(string token)
    {
        var auth = new AuthResponse();
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens
                .Any(t => t.Token == token));

        if (user == null)
        {
            auth.Message = "Invalid Token";
            return auth;
        }
        
        var refreshToken = user.RefreshTokens
            .Single(t => t.Token == token);

        if (!refreshToken.isActive)
        {
            auth.Message = "Inactive Token";
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
    }*/
    public async Task<AuthResponse> RefreshTokenCheckAsync(string token)
    {
        var auth = new AuthResponse();
        if (string.IsNullOrEmpty(token))
        {
            auth.Message = "Token is required";
            return auth;
        }
        
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens
                .Any(t => t.Token == token));

        if (user == null)
        {
            auth.Message = "Invalid Token";
            return auth;
        }
        
        var refreshToken = user.RefreshTokens.FirstOrDefault(t => t.Token == token);
        if (refreshToken == null || !refreshToken.isActive)
        {
            auth.Message = "Inactive Token";
            return auth;
        }
        
        user.RefreshTokens.Remove(refreshToken);
        
        var newRefreshToken = await GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);
        
        var jwtSecurityKey = await CreateJwtAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        
        auth.Email = user.Email;
        auth.Roles = roles.ToList();
        auth.Username = user.UserName;
        auth.IsAuthenticated = true;
        auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityKey);
        auth.TokenExpiresOn = jwtSecurityKey.ValidTo;
        auth.RefreshToken = newRefreshToken.Token;
        auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;
        
        return auth;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        /*var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
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
        
        return true;*/
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
    
        if (user == null)
            return false;

        var refreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == token);
        if (refreshToken == null || !refreshToken.isActive)
            return false;
        
        user.RefreshTokens.Remove(refreshToken);
    
        await _userManager.UpdateAsync(user);
    
        return true;
    }
}
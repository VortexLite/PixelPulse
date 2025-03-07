using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppIdentityContext _context;

    public UserRepository(ILogger<UserRepository> logger, UserManager<AppUser> userManager, AppIdentityContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _context = context;
    }

    public IQueryable<AppUser>? FindUserByRefreshToken(string token)
    {
        return _userManager.Users
            .Include(u => u.RefreshTokens)
            .Where(u => u.RefreshTokens
                .Any(t => t.Token == token));
    }

    public async Task<AppUser?> UpdateUserAsync(AppUser user, AppUser updatedUserRequest)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Entry(user).CurrentValues.SetValues(updatedUserRequest);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
        }

        return user;
    }
}
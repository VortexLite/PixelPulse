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
            if (!string.IsNullOrWhiteSpace(updatedUserRequest.FirstName))
                user.FirstName = updatedUserRequest.FirstName;

            if (!string.IsNullOrWhiteSpace(updatedUserRequest.LastName))
                user.LastName = updatedUserRequest.LastName;

            if (!string.IsNullOrWhiteSpace(updatedUserRequest.UserName))
                user.UserName = updatedUserRequest.UserName;

            if (!string.IsNullOrWhiteSpace(updatedUserRequest.Email))
                user.Email = updatedUserRequest.Email;

            if (!string.IsNullOrWhiteSpace(updatedUserRequest.PhoneNumber))
                user.PhoneNumber = updatedUserRequest.PhoneNumber;
            
            _context.Users.Update(user);
            //_context.Entry(user).CurrentValues.SetValues(updatedUserRequest);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
        }

        return user;
    }

    public IQueryable<AppUser> Get50UsersAsync(int page = 1)
    {
        int pageSize = 50;
        return _userManager.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }

    public async Task<int> CountAsync()
    {
        return await _userManager.Users.CountAsync();
    }

    public async Task<bool> DeleteUserAsync(AppUser entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.AppUsers.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public IQueryable<AppUser> GetUserById(string userId)
    {
        return _context.AppUsers
            .Where(u => u.Id == userId);
    }
}
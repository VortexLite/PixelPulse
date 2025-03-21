using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IUserRepository
{
    IQueryable<AppUser>? FindUserByRefreshToken(string token);
    Task<AppUser?> UpdateUserAsync(AppUser user, AppUser updatedUserRequest);
    IQueryable<AppUser> Get50UsersAsync(int page = 1);
    Task<int> CountAsync();
    Task<bool> DeleteUserAsync(AppUser user, CancellationToken cancellationToken = default);
    IQueryable<AppUser> GetUserById(string userId);
}
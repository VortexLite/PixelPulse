using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IUserRepository
{
    IQueryable<AppUser>? FindUserByRefreshToken(string token);
    Task<AppUser?> UpdateUserAsync(AppUser user, AppUser updatedUserRequest);
}
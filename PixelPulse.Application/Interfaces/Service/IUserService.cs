using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Application.DTOs.User;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Service;

public interface IUserService
{
    Task<AppUser?> FindUserByRefreshToken(string token);
    Task<AppUser?> UpdateUser(UserUpdateRequest userUpdateRequest);
    Task<UsersAndCountDTO> Get50UsersAsync(int page = 1);
    Task<bool> DeleteUserAsync(string userId);
}
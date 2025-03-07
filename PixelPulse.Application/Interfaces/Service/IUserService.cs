using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Service;

public interface IUserService
{
    Task<AppUser?> FindUserByRefreshToken(string token);
    Task<AppUser?> UpdateUser(UserUpdateRequest userUpdateRequest);
}
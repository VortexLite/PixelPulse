using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Service.User;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;

    public UserService(ILogger<UserService> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }
    
    public async Task<AppUser?> FindUserByRefreshToken(string token)
    {
        var user = await _userRepository
            .FindUserByRefreshToken(token)
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<AppUser?> UpdateUser(UserUpdateRequest userUpdateRequest)
    {
        var user = await _userRepository
            .FindUserByRefreshToken(userUpdateRequest.RefreshToken)
            .FirstOrDefaultAsync();
        if (user == null)
            return null;
        
        return await _userRepository.UpdateUserAsync(user, userUpdateRequest.AppUser);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Application.DTOs.User;
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

    public async Task<UsersAndCountDTO> Get50UsersAsync(int page = 1)
    {
        var users = await _userRepository
            .Get50UsersAsync(page)
            .ToListAsync();
        
        int totalItems = await _userRepository.CountAsync();
        var dto = new UsersAndCountDTO
        {
            AppUsers = users,
            CurrentPage = page,
            TotalPages = totalItems > 0 ? (int)Math.Ceiling((double)totalItems / 50) : 1,
            TotalItems = totalItems,
        };
        
        return dto;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userRepository
            .GetUserById(userId)
            .FirstOrDefaultAsync();
        if (user == null)
            return false;
        
        return await _userRepository.DeleteUserAsync(user);
    }
}
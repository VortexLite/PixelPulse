using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.PersonalInformation;
using PixelPulse.Application.DTOs.RefreshToken;
using PixelPulse.Application.Interfaces.Authentication;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IAuthResponse _authService;
    private readonly IUserService _userService;
    
    public UserController(ILogger<UserController> logger, IAuthResponse authService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _userService = userService;
    }
    
    
    [HttpPost("personal_Information")]
    public async Task<IActionResult> PersonalInformation([FromBody] RefreshTokenRequest refreshToken)
    {
        var user = await _userService.FindUserByRefreshToken(refreshToken.RefreshToken);
        if (user == null)
            return Unauthorized("Invalid refresh token");
        
        return Ok(user);
    }
    
    [HttpPost("save_personal_Information")]
    public async Task<IActionResult> Save_PersonalInformation([FromBody] UserUpdateRequest request)
    {
        var user = await _userService.UpdateUser(request);
        if (user == null)
            BadRequest("Invalid request");
        
        return Ok(user);
    }
}
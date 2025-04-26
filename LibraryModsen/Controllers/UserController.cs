using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts.User;
using LibraryModsen.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryModsen.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(
        IUserService userService,
        CurrentUser currentUser) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly CurrentUser _currentUser = currentUser;

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserResponse>> GetCurrentUser(CancellationToken cancelToken = default)
    {
        var userId = _currentUser.GetCurrentUserId();
        var result = await _userService.GetById(userId, cancelToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("notifications")]
    public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetNotifications(CancellationToken cancelToken = default)
    {
        var userId = _currentUser.GetCurrentUserId();
        var res = await _userService.GetNotifications(userId, cancelToken);
        return Ok(res);
    }
}

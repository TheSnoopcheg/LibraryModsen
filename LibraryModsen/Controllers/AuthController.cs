using Microsoft.AspNetCore.Mvc;
using LibraryModsen.Application.Contracts.Auth;
using LibraryModsen.Validators.Auth;
using Microsoft.AspNetCore.Authorization;
using LibraryModsen.Application.Abstractions.Services;

namespace LibraryModsen.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
        IAuthService userService,
        LoginRequestValidator lrValidator,
        RegisterRequestValidator rrValidator) : ControllerBase
{
    private readonly IAuthService _userService = userService;
    private readonly LoginRequestValidator _lrValidator = lrValidator;
    private readonly RegisterRequestValidator _rrValidator = rrValidator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var validationRes = await _lrValidator.ValidateAsync(request);
        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors);
        }
        if(!await _userService.ExistsWithEmail(request.Email))
        {
            return NotFound();
        }
        var token = await _userService.Login(request);
        if (string.IsNullOrEmpty(token.Item1) || string.IsNullOrEmpty(token.Item2))
        {
            return Forbid();
        }

        HttpContext.Response.Cookies.Append("token_k", token.Item1, new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            MaxAge = TimeSpan.FromMinutes(30),
            SameSite = SameSiteMode.Strict
        });
        HttpContext.Response.Cookies.Append("token_r", token.Item2, new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            MaxAge = TimeSpan.FromDays(7),
            SameSite = SameSiteMode.Strict,
            Path = Url.RouteUrl("RefreshTokenRoute")
        });

        return Ok();
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        var validationRes = await _rrValidator.ValidateAsync(request);
        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors);
        }
        if(await _userService.ExistsWithEmail(request.Email))
        {
            return NoContent();
        }
        var errors = await _userService.Register(request);
        if(errors == null)
        {
            return Ok();
        }
        return StatusCode(500, errors);
    }

    [HttpPost("refresh-token/{userId}", Name = "RefreshTokenRoute")]
    public async Task<ActionResult> RefreshToken(Guid userId)
    {
        var refreshToken = HttpContext.Request.Cookies["token_r"];
        if (string.IsNullOrEmpty(refreshToken)) return Unauthorized("Invalid refresh token");
        var newToken = await _userService.RefreshToken(userId, refreshToken);
        HttpContext.Response.Cookies.Append("token_k", newToken, new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            MaxAge = TimeSpan.FromMinutes(30),
            SameSite = SameSiteMode.Strict
        });
        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public void Logout()
    {
        HttpContext.Response.Cookies.Delete("token_r");
        HttpContext.Response.Cookies.Delete("token_k");
    }
}

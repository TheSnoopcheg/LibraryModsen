using AutoMapper;
using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts.Auth;
using LibraryModsen.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Application.Services;

public class AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMapper mapper,
        ITokenService tokenService) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> ExistsWithEmail(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<IEnumerable<IdentityError>?> Register(RegisterRequest request)
    {
        var user = _mapper.Map<User>(request);
        var createUser = await _userManager.CreateAsync(user, request.Password);
        if (createUser.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (roleResult.Succeeded)
            {
                return null;
            }
            return roleResult.Errors;
        }
        return createUser.Errors;
    }

    public async Task<(string, string)> Login(LoginRequest request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        var res = await _signInManager.CheckPasswordSignInAsync(user!, request.Password, false);
        if (res.Succeeded)
        {
            return (await _tokenService.GenerateToken(user!), await _tokenService.AssignRefreshTokenToUser(user!));
        }
        return (string.Empty, string.Empty);
    }

    public async Task<string> RefreshToken(Guid userId, string refreshToken)
    {
        var user = await _tokenService.ValidateRefreshToken(userId, refreshToken);
        if (user == null) return string.Empty;
        return await _tokenService.GenerateToken(user);
    }
}

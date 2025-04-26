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

    public async Task<bool> ExistsWithEmail(string email, CancellationToken cancelToken = default)
    {
        return await _userManager.Users.AnyAsync(x => x.Email == email, cancelToken);
    }

    public async Task Register(RegisterRequest request, CancellationToken cancelToken = default)
    {
        if (await ExistsWithEmail(request.Email, cancelToken))
            throw new Exception("User with such email already exists");
        var user = _mapper.Map<User>(request);
        var createUser = await _userManager.CreateAsync(user, request.Password);
        if (createUser.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (roleResult.Succeeded)
            {
                return;
            }
            throw new Exception("Unable to add role to user");
        }
        throw new Exception("Unable to create user");
    }

    public async Task<(string, string)> Login(LoginRequest request, CancellationToken cancelToken = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancelToken);
        if (user == null)
            throw new Exception("User not found");
        var res = await _signInManager.CheckPasswordSignInAsync(user!, request.Password, false);
        if (res.Succeeded)
        {
            return (await _tokenService.GenerateToken(user!), await _tokenService.AssignRefreshTokenToUser(user!));
        }
        return (string.Empty, string.Empty);
    }

    public async Task<string> RefreshToken(Guid userId, string refreshToken, CancellationToken cancelToken = default)
    {
        var user = await _tokenService.ValidateRefreshToken(userId, refreshToken, cancelToken);
        if (user == null) return string.Empty;
        return await _tokenService.GenerateToken(user);
    }
}

using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Common;
using LibraryModsen.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryModsen.Application.Services;

public class TokenService(
        IOptions<JwtOptions> options,
        UserManager<User> userManager) : ITokenService
{
    private readonly JwtOptions _options = options.Value;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<string> GenerateToken(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!)
        };
        claims.AddRange(userRoles.Select(o => new Claim("Policy", o)));
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpirationTime));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User?> ValidateRefreshToken(Guid userId, string refreshToken, CancellationToken cancelToken = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancelToken);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return null;
        return user;
    }

    public async Task<string> AssignRefreshTokenToUser(User user)
    {
        var token = GenerateRefreshToken();
        user.RefreshToken = token;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);
        return token;
    }
    private string GenerateRefreshToken()
    {
        Span<byte> randomNumbers = stackalloc byte[32];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(randomNumbers);
        }
        return Convert.ToBase64String(randomNumbers);
    }
}

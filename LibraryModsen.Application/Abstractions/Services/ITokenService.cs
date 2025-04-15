using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Services
{
    public interface ITokenService
    {
        Task<string> AssignRefreshTokenToUser(User user);
        Task<string> GenerateToken(User user);
        Task<User?> ValidateRefreshToken(Guid userId, string refreshToken);
    }
}
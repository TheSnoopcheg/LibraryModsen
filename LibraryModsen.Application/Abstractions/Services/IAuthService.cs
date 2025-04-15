using LibraryModsen.Application.Contracts.Auth;
using Microsoft.AspNetCore.Identity;

namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<bool> ExistsWithEmail(string email);
        Task<(string, string)> Login(LoginRequest request);
        Task<string> RefreshToken(Guid userId, string refreshToken);
        Task<IEnumerable<IdentityError>?> Register(RegisterRequest request);
    }
}
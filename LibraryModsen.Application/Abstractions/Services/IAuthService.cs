using LibraryModsen.Application.Contracts.Auth;
using Microsoft.AspNetCore.Identity;

namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<bool> ExistsWithEmail(string email, CancellationToken cancelToken = default);
        Task<(string, string)> Login(LoginRequest request, CancellationToken cancelToken = default);
        Task<string> RefreshToken(Guid userId, string refreshToken, CancellationToken cancelToken = default);
        Task Register(RegisterRequest request, CancellationToken cancelToken = default);
    }
}
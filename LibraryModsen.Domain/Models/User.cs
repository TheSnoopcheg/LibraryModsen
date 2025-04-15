using Microsoft.AspNetCore.Identity;

namespace LibraryModsen.Domain.Models;

public class User : IdentityUser<Guid>
{
    public List<BookState> TakenBooks { get; set; } = [];
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
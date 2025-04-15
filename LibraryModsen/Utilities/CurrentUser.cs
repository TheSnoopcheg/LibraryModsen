using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LibraryModsen.Utilities;

public class CurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Guid GetCurrentUserId()
    {
        var tokenstr = GetTokenString();
        var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenstr);
        var userId = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        return Guid.Parse(userId);
    }
    public string? GetTokenString()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["token_k"];
        return token;
    }
}

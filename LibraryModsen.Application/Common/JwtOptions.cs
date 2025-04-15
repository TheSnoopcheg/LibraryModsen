namespace LibraryModsen.Application.Common;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpirationTime { get; set; }
}

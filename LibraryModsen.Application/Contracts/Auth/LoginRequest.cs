namespace LibraryModsen.Application.Contracts.Auth;

public record class LoginRequest(
    string Email, 
    string Password);
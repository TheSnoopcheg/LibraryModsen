namespace LibraryModsen.Application.Contracts.Auth;

public record RegisterRequest(
    string UserName, 
    string Email, 
    string Password);
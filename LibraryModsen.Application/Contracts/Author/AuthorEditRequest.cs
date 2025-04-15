namespace LibraryModsen.Application.Contracts.Author;

public record class AuthorEditRequest(
    Guid Id,
    string Name,
    string Surname,
    DateTime DateOfBirth,
    string Country);
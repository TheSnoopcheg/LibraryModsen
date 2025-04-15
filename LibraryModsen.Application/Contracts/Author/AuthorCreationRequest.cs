namespace LibraryModsen.Application.Contracts.Author;

public record class AuthorCreationRequest(
    string Name,
    string Surname,
    DateTime DateOfBirth,
    string Country);

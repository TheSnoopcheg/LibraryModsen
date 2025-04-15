using LibraryModsen.Application.Contracts.Book;

namespace LibraryModsen.Application.Contracts.Author;

public record class AuthorResponse(
    Guid Id,
    string Name,
    string Surname,
    string Country);

public record class AuthorFullResponse(
    Guid Id,
    string Name,
    string Surname,
    DateTime DateOfBirth,
    string Country,
    List<BookResponse> Books);

using LibraryModsen.Application.Contracts.Author;

namespace LibraryModsen.Application.Contracts.Book;

public record class BookFullResponse(
    Guid Id,
    string ISBN,
    string Title,
    string Genre,
    string Discription,
    string CoverLink,
    List<AuthorResponse> Authors);
public record class BookResponse(
    Guid Id,
    string ISBN,
    string Title,
    string Genre,
    string Discription,
    string CoverLink);

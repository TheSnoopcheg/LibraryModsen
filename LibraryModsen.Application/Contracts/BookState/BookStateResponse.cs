using LibraryModsen.Application.Contracts.Book;

namespace LibraryModsen.Application.Contracts.BookState;

public record class BookStateResponse(
    Guid Id,
    bool IsTaken,
    BookResponse Book,
    DateTime TakingTime,
    DateTime ExpirationTime);

using LibraryModsen.Application.Contracts.BookState;

namespace LibraryModsen.Application.Contracts.User;

public record class UserResponse(
    Guid Id,
    string UserName,
    string Email,
    List<BookStateResponse> TakenBooks);

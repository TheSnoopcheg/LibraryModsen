using System.ComponentModel.DataAnnotations;

namespace LibraryModsen.Application.Contracts.Book;

public record class BookEditRequest(
    [Required]
    Guid Id,
    string ISBN,
    string Title,
    string Genre,
    string Discription,
    List<Guid> AuthorsId);
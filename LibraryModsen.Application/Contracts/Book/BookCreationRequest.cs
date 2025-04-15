using System.ComponentModel.DataAnnotations;

namespace LibraryModsen.Application.Contracts.Book;

public record class BookCreationRequest(
    [Required]
    string ISBN,
    [Required]
    string Title,
    [Required]
    string Genre,
    string Discription,
    [Required]
    List<Guid> AuthorsId,
    int NumOfBooks);
namespace LibraryModsen.Domain.Models;

public class Book
{
    public Guid Id { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Discription { get; set; } = string.Empty;
    public List<Author> Authors { get; set; } = [];
    public string? CoverLink { get; set; } = string.Empty;
    public List<BookState> BookStates { get; set; } = [];
}

namespace LibraryModsen.Domain.Models;

public class BookState
{
    public Guid Id { get; set; }
    public bool IsTaken { get; set; }
    public Guid BookId { get; set; }
    public Book? Book { get; set; }
    public Guid? HolderId { get; set; }
    public User? Holder { get; set; }
    public DateTime TakingTime { get; set; }
    public DateTime ExpirationTime { get; set; }
}

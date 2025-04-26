using LibraryModsen.Domain.Models;
using LibraryModsen.Persistence;
using LibraryModsen.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Tests.TestCase;

public class BooksRepositoryTest
{
    private readonly LibraryDbContext _context;
    public BooksRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new LibraryDbContext(options);
    }

    [Fact]
    public async Task BooksRepository_AddBook_ShouldAddBook()
    {
        Guid id = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1");
        var book = new Book
        {
            Id = id,
            Title = "Clear architecture",
            ISBN = "1234567890",
            CoverLink = "Somelinkhere",
            Genre = "Fairy tales",
            Discription = "The best fairy tale in the world",
            Authors = []
        };

        var repository = new BooksRepository(_context);

        var result = await repository.Add(book, default);

        Assert.Equal(id, result);
    }

    [Fact]
    public async Task BooksRepository_GetById_ShouldReturnABook()
    {
        var id = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1");
        var book = new Book
        {
            Id = id,
            Title = "Clear architecture",
            ISBN = "1234567890",
            CoverLink = "Somelinkhere",
            Genre = "Fairy tales",
            Discription = "The best fairy tale in the world"
        };
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        var repository = new BooksRepository(_context);

        var result = await repository.GetById(id);

        Assert.NotNull(result);

        Assert.Equal(book.Id, result.Id);
        Assert.Equal(book.Title, result.Title);
        Assert.Equal(book.ISBN, result.ISBN);
        Assert.Equal(book.CoverLink, result.CoverLink);
        Assert.Equal(book.Genre, result.Genre);
        Assert.Equal(book.Discription, result.Discription);
    }
}

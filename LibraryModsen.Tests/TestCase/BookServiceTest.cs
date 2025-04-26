using AutoMapper;
using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts;
using LibraryModsen.Application.Contracts.Book;
using LibraryModsen.Application.Services;
using LibraryModsen.Domain.Models;
using Moq;

namespace LibraryModsen.Tests.TestCase;

public class BookServiceTest
{
    private readonly Mock<IBooksRepository> _repository = new();
    private readonly Mock<IBookStateService> _bookStateService = new();
    private readonly Mock<IMapper> _mapper = new();

    [Fact]
    public async Task BookService_AnyById_ShouldReturnTrue()
    {
        Guid id = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1");
        var book = new Book
        {
            Id = id,
            Title = "Clear architecture",
            ISBN = "1234567890",
            CoverLink = "Somelinkhere",
            Genre="Fairy tales"
        };
        _repository.Setup(r => r.Any(id, default)).ReturnsAsync(true);
        var bookservice = new BookService(_repository.Object, _bookStateService.Object, _mapper.Object);

        var result = await bookservice.Any(id);

        Assert.True(result);
    }

    [Fact]
    public async Task BookService_GetAll_ShouldReturnBooks()
    {
        int count = 3;
        var request = new FilterRequest();
        var expectedBooks = new List<Book>
        {
            new Book
            {
                Id = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1"),
                ISBN = "1234567890",
                Title = "C#",
                Genre = "Fairy Tales",
                Discription = "C#",
                CoverLink = "SomeLink",
                Authors = []
            },
            new Book
            {
                Id = Guid.Parse("9f1274ac-024d-4a36-98eb-e72ae5732590"),
                ISBN = "1234567891",
                Title = "C++",
                Genre = "Fairy Tales",
                Discription = "C++",
                CoverLink = "SomeLink",
                Authors = []
            },
            new Book
            {
                Id = Guid.Parse("af2bfb45-1323-4ad6-876a-cfc1d1a39c70"),
                ISBN = "1234567892",
                Title = "C",
                Genre = "Fairy Tales",
                Discription = "C",
                CoverLink = "SomeLink",
                Authors = []
            }
        };
        var expectedResult = new List<BookFullResponse>
        {
            new BookFullResponse
            (
                Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1"),
                "1234567890",
                "C#",
                "Docs",
                "C#",
                "SomeLink",
                []
            ),
            new BookFullResponse
            (
                Guid.Parse("9f1274ac-024d-4a36-98eb-e72ae5732590"),
                "1234567891",
                "C++",
                "Fairy Tales",
                "C++",
                "SomeLink",
                []
            ),
            new BookFullResponse
            (
                Guid.Parse("af2bfb45-1323-4ad6-876a-cfc1d1a39c70"),
                "1234567892",
                "C",
                "Fairy Tales",
                "C",
                "SomeLink",
                []
            )
        };

        _repository.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(expectedBooks);
        _mapper.Setup(m => m.Map<BookFullResponse>(expectedBooks[0])).Returns(expectedResult[0]);
        _mapper.Setup(m => m.Map<BookFullResponse>(expectedBooks[1])).Returns(expectedResult[1]);
        _mapper.Setup(m => m.Map<BookFullResponse>(expectedBooks[2])).Returns(expectedResult[2]);

        var bookService = new BookService(_repository.Object, _bookStateService.Object, _mapper.Object);

        var result = await bookService.GetAll(request);

        Assert.NotNull(result);
        Assert.Equal(count, result.Count());

        var actual = result.ToList();

        for (int i = 0; i < count; i++)
        {
            Assert.Equal(expectedResult[i].Id, actual[i].Id);
            Assert.Equal(expectedResult[i].ISBN, actual[i].ISBN);
            Assert.Equal(expectedResult[i].Title, actual[i].Title);
            Assert.Equal(expectedResult[i].Genre, actual[i].Genre);
            Assert.Equal(expectedResult[i].Discription, actual[i].Discription);
            Assert.Equal(expectedResult[i].CoverLink, actual[i].CoverLink);
            Assert.Equal(expectedResult[i].Authors.Count, actual[i].Authors.Count);
        }
    }

    [Fact]
    public async Task BookService_GetById_ShouldReturnABook()
    {
        Guid id = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1");
        var expectBook = new Book
        {
            Id = id,
            ISBN = "1234567890",
            Title = "C#",
            Genre = "Fairy Tales",
            Discription = "C#",
            CoverLink = "SomeLink",
            Authors = []
        };
        var expectResult = new BookFullResponse
        (
            id,
            "1234567890",
            "C#",
            "Docs",
            "C#",
            "SomeLink",
            []
        );

        _repository.Setup(r => r.GetById(id, default)).ReturnsAsync(expectBook);
        _mapper.Setup(m => m.Map<BookFullResponse>(expectBook)).Returns(expectResult);
        var bookService = new BookService(_repository.Object, _bookStateService.Object, _mapper.Object);

        var result = await bookService.GetById(id);

        Assert.NotNull(result);
        Assert.Equal(expectResult.Id, result.Id);
        Assert.Equal(expectResult.ISBN, result.ISBN);
        Assert.Equal(expectResult.Title, result.Title);
        Assert.Equal(expectResult.Genre, result.Genre);
        Assert.Equal(expectResult.Discription, result.Discription);
        Assert.Equal(expectResult.CoverLink, result.CoverLink);
        Assert.Equal(expectResult.Authors.Count, result.Authors.Count);
    }

    [Fact]
    public async Task BookService_GetByISBN_ShouldReturnABook()
    {
        string isbn = "1234567890";
        var expectBook = new Book
        {
            Id = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1"),
            ISBN = isbn,
            Title = "C#",
            Genre = "Fairy Tales",
            Discription = "C#",
            CoverLink = "SomeLink",
            Authors = []
        };
        var expectResult = new BookFullResponse
        (
            Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1"),
            isbn,
            "C#",
            "Docs",
            "C#",
            "SomeLink",
            []
        );

        _repository.Setup(r => r.GetByISBN(isbn, default)).ReturnsAsync(expectBook);
        _mapper.Setup(m => m.Map<BookFullResponse>(expectBook)).Returns(expectResult);
        var bookService = new BookService(_repository.Object, _bookStateService.Object, _mapper.Object);

        var result = await bookService.GetByISBN(isbn);

        Assert.NotNull(result);
        Assert.Equal(expectResult.Id, result.Id);
        Assert.Equal(expectResult.ISBN, result.ISBN);
        Assert.Equal(expectResult.Title, result.Title);
        Assert.Equal(expectResult.Genre, result.Genre);
        Assert.Equal(expectResult.Discription, result.Discription);
        Assert.Equal(expectResult.CoverLink, result.CoverLink);
        Assert.Equal(expectResult.Authors.Count, result.Authors.Count);
    }

    [Fact]
    public async Task BookService_CreateBook_ShouldAddBook()
    {
        Guid expect = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1");
        string coverLink = "link";

        var request = new BookCreationRequest
            (
                "1234567890",
                "C#",
                "Fairy Tales",
                "C#",
                [],
                1
            );

        var expectedBook = new Book
        {
            Id = expect,
            ISBN = "1234567890",
            Title = "C#",
            Genre = "Fairy Tales",
            Discription = "C#",
            CoverLink = coverLink
        };

        _mapper.Setup(m => m.Map<Book>(request)).Returns(expectedBook);

        _repository.Setup(r => r.Add(It.IsAny<Book>(), new List<Guid>(), default));

        var bookService = new BookService(_repository.Object, _bookStateService.Object, _mapper.Object);

        await bookService.CreateBook(request, coverLink);

        _repository.Verify(r => r.Add(It.IsAny<Book>(), new List<Guid>(), default), Times.Once);
    }

    [Fact]
    public async Task BookService_EditBook_ShouldEditBook()
    {
        Guid expect = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1");
        string coverLink = "link";

        var request = new BookEditRequest
            (
                Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1"),
                "1234567890",
                "C#",
                "Fairy Tales",
                "C#",
                []
            );

        var expectedBook = new Book
        {
            Id = expect,
            ISBN = "1234567890",
            Title = "C#",
            Genre = "Fairy Tales",
            Discription = "C#",
            CoverLink = coverLink,
            Authors = []
        };

        _mapper.Setup(m => m.Map<Book>(request)).Returns(expectedBook);
        _repository.Setup(r => r.Update(It.IsAny<Book>(), new List<Guid>(), default));
        _repository.Setup(r => r.Any(It.IsAny<Guid>(), default)).ReturnsAsync(true);

        var bookService = new BookService(_repository.Object, _bookStateService.Object, _mapper.Object);

        await bookService.EditBook(request, coverLink);

        _repository.Verify(r => r.Update(It.IsAny<Book>(), new List<Guid>(), default), Times.Once);
    }

    [Fact]
    public async Task BookService_DeleteBook_ShouldDeleteBook()
    {
        Guid expect = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1");

        _repository.Setup(r => r.Any(It.IsAny<Guid>(), default)).ReturnsAsync(true);

        _repository.Setup(r => r.Delete(It.IsAny<Guid>(), default));

        var bookService = new BookService(_repository.Object, _bookStateService.Object, _mapper.Object);

        await bookService.DeleteBook(expect);

        _repository.Verify(r => r.Delete(It.IsAny<Guid>(), default), Times.Once);
    }

    [Theory]
    [InlineData(1,3,3)]
    [InlineData(2,3,0)]
    [InlineData(1,2,2)]
    public async Task BookService_GetPage_ShouldReturnBooks(int page, int size, int expected)
    {
        var request = new FilterRequest();
        var expectedBooks = new List<Book>
        {
            new Book
            {
                Id = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1"),
                ISBN = "1234567890",
                Title = "C#",
                Genre = "Fairy Tales",
                Discription = "C#",
                CoverLink = "SomeLink",
                Authors = []
            },
            new Book
            {
                Id = Guid.Parse("9f1274ac-024d-4a36-98eb-e72ae5732590"),
                ISBN = "1234567891",
                Title = "C++",
                Genre = "Fairy Tales",
                Discription = "C++",
                CoverLink = "SomeLink",
                Authors = []
            },
            new Book
            {
                Id = Guid.Parse("af2bfb45-1323-4ad6-876a-cfc1d1a39c70"),
                ISBN = "1234567892",
                Title = "C",
                Genre = "Fairy Tales",
                Discription = "C",
                CoverLink = "SomeLink",
                Authors = []
            }
        };
        var expectedResult = new List<BookFullResponse>
        {
            new BookFullResponse
            (
                Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1"),
                "1234567890",
                "C#",
                "Docs",
                "C#",
                "SomeLink",
                []
            ),
            new BookFullResponse
            (
                Guid.Parse("9f1274ac-024d-4a36-98eb-e72ae5732590"),
                "1234567891",
                "C++",
                "Fairy Tales",
                "C++",
                "SomeLink",
                []
            ),
            new BookFullResponse
            (
                Guid.Parse("af2bfb45-1323-4ad6-876a-cfc1d1a39c70"),
                "1234567892",
                "C",
                "Fairy Tales",
                "C",
                "SomeLink",
                []
            )
        };

        _repository.Setup(r => r.GetPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(expectedBooks.Skip(size * (page-1)).Take(size));
        _mapper.Setup(m => m.Map<BookFullResponse>(expectedBooks[0])).Returns(expectedResult[0]);
        _mapper.Setup(m => m.Map<BookFullResponse>(expectedBooks[1])).Returns(expectedResult[1]);
        _mapper.Setup(m => m.Map<BookFullResponse>(expectedBooks[2])).Returns(expectedResult[2]);

        var bookService = new BookService(_repository.Object, _bookStateService.Object, _mapper.Object);

        var result = await bookService.GetPage(page, size, request);

        Assert.NotNull(result);
        Assert.Equal(expected, result.Count());

        var actual = result.ToList();

        for (int i = (page - 1) * size; i < (page-1) * size + actual.Count; i++)
        {
            Assert.Equal(expectedResult[i].Id, actual[i].Id);
            Assert.Equal(expectedResult[i].ISBN, actual[i].ISBN);
            Assert.Equal(expectedResult[i].Title, actual[i].Title);
            Assert.Equal(expectedResult[i].Genre, actual[i].Genre);
            Assert.Equal(expectedResult[i].Discription, actual[i].Discription);
            Assert.Equal(expectedResult[i].CoverLink, actual[i].CoverLink);
            Assert.Equal(expectedResult[i].Authors.Count, actual[i].Authors.Count);
        }
    }
    [Fact]
    public async Task BookService_GetByTitle_ShouldReturnBooks()
    {
        string title = "Programming";
        int expectedCount = 3;
        var expectedBooks = new List<Book>
        {
            new Book
            {
                Id = Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1"),
                ISBN = "1234567890",
                Title = "Programming Language C#",
                Genre = "Fairy Tales",
                Discription = "C#",
                CoverLink = "SomeLink",
                Authors = []
            },
            new Book
            {
                Id = Guid.Parse("9f1274ac-024d-4a36-98eb-e72ae5732590"),
                ISBN = "1234567891",
                Title = "Programming Language C++",
                Genre = "Fairy Tales",
                Discription = "C++",
                CoverLink = "SomeLink",
                Authors = []
            },
            new Book
            {
                Id = Guid.Parse("af2bfb45-1323-4ad6-876a-cfc1d1a39c70"),
                ISBN = "1234567892",
                Title = "Programming Language C",
                Genre = "Fairy Tales",
                Discription = "C",
                CoverLink = "SomeLink",
                Authors = []
            }
        };
        var expectedResult = new List<BookFullResponse>
        {
            new BookFullResponse
            (
                Guid.Parse("0358f2cb-84f5-45d2-b395-6586c49842e1"),
                "1234567890",
                "Programming Language C#",
                "Docs",
                "C#",
                "SomeLink",
                []
            ),
            new BookFullResponse
            (
                Guid.Parse("9f1274ac-024d-4a36-98eb-e72ae5732590"),
                "1234567891",
                "Programming Language C++",
                "Fairy Tales",
                "C++",
                "SomeLink",
                []
            ),
            new BookFullResponse
            (
                Guid.Parse("af2bfb45-1323-4ad6-876a-cfc1d1a39c70"),
                "1234567892",
                "Programming Language C",
                "Fairy Tales",
                "C",
                "SomeLink",
                []
            )
        };

        _repository.Setup(r => r.GetByTitle(title, default)).ReturnsAsync(expectedBooks);
        _mapper.Setup(m => m.Map<BookFullResponse>(expectedBooks[0])).Returns(expectedResult[0]);
        _mapper.Setup(m => m.Map<BookFullResponse>(expectedBooks[1])).Returns(expectedResult[1]);
        _mapper.Setup(m => m.Map<BookFullResponse>(expectedBooks[2])).Returns(expectedResult[2]);
        var bookService = new BookService(_repository.Object, _bookStateService.Object, _mapper.Object);

        var result = await bookService.GetByTitle(title);

        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.Count());

        var actual = result.ToList();

        for (int i = 0; i < expectedCount; i++)
        {
            Assert.Equal(expectedResult[i].Id, actual[i].Id);
            Assert.Equal(expectedResult[i].ISBN, actual[i].ISBN);
            Assert.Equal(expectedResult[i].Title, actual[i].Title);
            Assert.Equal(expectedResult[i].Genre, actual[i].Genre);
            Assert.Equal(expectedResult[i].Discription, actual[i].Discription);
            Assert.Equal(expectedResult[i].CoverLink, actual[i].CoverLink);
            Assert.Equal(expectedResult[i].Authors.Count, actual[i].Authors.Count);
        }
    }
}

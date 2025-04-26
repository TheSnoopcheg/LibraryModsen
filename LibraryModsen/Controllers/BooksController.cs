using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts;
using LibraryModsen.Application.Contracts.Book;
using LibraryModsen.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryModsen.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(
        IBookService booksService,
        IFilesService filesService,
        IBookStateService bookStateService,
        CurrentUser currentUser) : ControllerBase
{
    private readonly IBookService _bookService = booksService;
    private readonly IFilesService _filesService = filesService;
    private readonly IBookStateService _bookStateService = bookStateService;
    private readonly CurrentUser _currentUser = currentUser;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookFullResponse>>> Get([FromQuery] FilterRequest request, CancellationToken cancelToken = default)
    {
        var books = await _bookService.GetAll(request, cancelToken);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookFullResponse>> GetById(Guid id, CancellationToken cancelToken = default)
    {
        var book = await _bookService.GetById(id, cancelToken);
        return Ok(book);
    }

    [HttpGet("isbn/{isbn}")]
    public async Task<ActionResult<BookFullResponse>> Get([AutoValidateAlways] string isbn, CancellationToken cancelToken = default)
    {
        var book = await _bookService.GetByISBN(isbn, cancelToken);
        return Ok(book);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost("{bookId}/add")]
    public async Task<IActionResult> AddStates(Guid bookId, [FromQuery(Name = "n")] int n, CancellationToken cancelToken = default)
    {
        await _bookStateService.AddBooks(bookId, n, cancelToken);
        return Ok();
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete("{bookStateId}/remove")]
    public async Task<IActionResult> RemoveState(Guid bookStateId, CancellationToken cancelToken = default)
    {
        await _bookStateService.Delete(bookStateId, cancelToken);
        return Ok();
    }

    [Authorize]
    [HttpPost("{bookId}/take")]
    public async Task<IActionResult> TakeBook(Guid bookId, CancellationToken cancelToken = default)
    {
        var userId = _currentUser.GetCurrentUserId();
        await _bookStateService.TakeBook(userId, bookId, cancelToken);
        return Ok();
    }

    [Authorize]
    [HttpPost("{bookStateId}/extend")]
    public async Task<IActionResult> ExtendBook(Guid bookStateId, [FromQuery(Name = "d")] int numofdays, CancellationToken cancelToken = default)
    {
        var userId = _currentUser.GetCurrentUserId();
        await _bookStateService.ExtendBook(bookStateId, userId, numofdays, cancelToken);
        return Ok();
    }

    [Authorize]
    [HttpPost("{bookStateId}/return")]
    public async Task<IActionResult> ReturnBook(Guid bookStateId, CancellationToken cancelToken = default)
    {
        var userId = _currentUser.GetCurrentUserId();
        await _bookStateService.ReturnBook(bookStateId, userId, cancelToken);
        return Ok();
    } 

    [HttpGet("page")]
    public async Task<ActionResult<IEnumerable<BookFullResponse>>> Get([FromQuery(Name = "p")] int page, [FromQuery(Name = "s")] int size, [FromQuery] FilterRequest request, CancellationToken cancelToken = default)
    {
        var books = await _bookService.GetPage(page, size, request, cancelToken);
        return Ok(books);
    }

    [HttpGet("cover/{guid}", Name = "GetCoverRoute")]
    public async Task<ActionResult> GetCover(Guid guid, CancellationToken cancelToken = default)
    {
        var file = await _filesService.GetFileById(guid, cancelToken);
        var ms = new MemoryStream(file!.Data);
        return File(ms, file.Type);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] BookCreationRequest request, [AutoValidateAlways] IFormFile? file, CancellationToken cancelToken = default)
    {
        string coverLink = string.Empty;
        if(file != null)
        {
            var guid = await _filesService.CreateFile(file.OpenReadStream(), file.ContentType, cancelToken);
            if (guid != Guid.Empty)
                coverLink = Url.Link("GetCoverRoute", new { Guid = guid })!;
        }
        await _bookService.CreateBook(request, coverLink, cancelToken);
        return Ok();
    }

    [Authorize(Policy = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Put([FromForm] BookEditRequest request, [AutoValidateAlways] IFormFile? file, CancellationToken cancelToken = default)
    {
        string coverLink = string.Empty;
        if (file != null)
        {
            var guid = await _filesService.CreateFile(file.OpenReadStream(), file.ContentType, cancelToken);
            if (guid != Guid.Empty)
                coverLink = Url.Link("GetCoverRoute", new { Guid = guid })!;
        }
        await _bookService.EditBook(request, coverLink, cancelToken);
        return Ok();
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancelToken = default)
    {
        await _bookService.DeleteBook(id, cancelToken);
        return Ok();
    }

    [HttpGet("{bookid}/available-num")]
    public async Task<ActionResult<int>> GetNumOfAvailableBooks(Guid bookid, CancellationToken cancelToken = default)
    {
        var res = await _bookStateService.GetAvailableNumOfBooks(bookid, cancelToken);
        return Ok(res);
    }
    
    [HttpGet("title/{title}")]
    public async Task<ActionResult<IEnumerable<BookFullResponse>>> GetByTitle([MinLength(1)] string title, CancellationToken cancelToken = default)
    {
        var books = await _bookService.GetByTitle(title, cancelToken);
        return Ok(books);
    }
}

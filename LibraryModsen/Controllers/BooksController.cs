using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts;
using LibraryModsen.Application.Contracts.Book;
using LibraryModsen.Utilities;
using LibraryModsen.Validators;
using LibraryModsen.Validators.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryModsen.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(
        IBookService booksService,
        IFilesService filesService,
        IBookStateService bookStateService,
        CurrentUser currentUser,
        ISBNValidator isbnValidator,
        BookCreationRequestValidator bcrValidator,
        FileValidator fileValidator,
        BookEditRequestValidator berValidator) : ControllerBase
{
    private readonly IBookService _bookService = booksService;
    private readonly IFilesService _filesService = filesService;
    private readonly IBookStateService _bookStateService = bookStateService;
    private readonly CurrentUser _currentUser = currentUser;
    private readonly ISBNValidator _isbnValidator = isbnValidator;
    private readonly BookCreationRequestValidator _bcrValidator = bcrValidator;
    private readonly FileValidator _fileValidator = fileValidator;
    private readonly BookEditRequestValidator _berValidator = berValidator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookFullResponse>>> Get([FromQuery] FilterRequest request)
    {
        if(request.FilterType == Application.Common.FilterType.Author)
        {
            if(!Guid.TryParse(request.Data, out _))
            {
                return BadRequest("Invalid author Id");
            }
        }
        var books = await _bookService.GetAll(request);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookFullResponse>> GetById(Guid id)
    {
        var book = await _bookService.GetById(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpGet("isbn/{isbn}")]
    public async Task<ActionResult<BookFullResponse>> Get(string isbn)
    {
        var validationRes = await _isbnValidator.ValidateAsync(isbn);
        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors);
        }
        var book = await _bookService.GetByISBN(isbn);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost("{bookId}/add")]
    public async Task<ActionResult> AddStates(Guid bookId, [FromQuery(Name = "n")] int n)
    {
        await _bookStateService.AddBooks(bookId, n);
        return Ok();
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete("{bookStateId}/remove")]
    public async Task<ActionResult> RemoveState(Guid bookStateId)
    {
        await _bookStateService.Delete(bookStateId);
        return Ok();
    }

    [Authorize]
    [HttpPost("{bookId}/take")]
    public async Task<ActionResult> TakeBook(Guid bookId)
    {
        var userId = _currentUser.GetCurrentUserId();
        var res = await _bookStateService.TakeBook(userId, bookId);
        if(res == Guid.Empty)
        {
            return NoContent();
        }
        return Ok(res);
    }

    [Authorize]
    [HttpPost("{bookStateId}/extend")]
    public async Task<ActionResult<Guid>> ExtendBook(Guid bookStateId, [FromQuery(Name = "d")] int numofdays)
    {
        var userId = _currentUser.GetCurrentUserId();
        var res = await _bookStateService.ExtendBook(bookStateId, userId, numofdays);
        if(res == Guid.Empty)
        {
            return NotFound();
        }
        return Ok(res);
    }

    [Authorize]
    [HttpPost("{bookStateId}/return")]
    public async Task<ActionResult<Guid>> ReturnBook(Guid bookStateId)
    {
        var userId = _currentUser.GetCurrentUserId();
        var res = await _bookStateService.ReturnBook(bookStateId, userId);
        if(res == Guid.Empty)
        {
            return NotFound();
        }
        return Ok(res);
    } 

    [HttpGet("page")]
    public async Task<ActionResult<IEnumerable<BookFullResponse>>> Get([FromQuery(Name = "p")] int page, [FromQuery(Name = "s")] int size, [FromQuery] FilterRequest request)
    {
        if(page < 1 || size < 1)
        {
            return BadRequest();
        }
        var books = await _bookService.GetPage(page - 1, size, request);
        return Ok(books);
    }

    [HttpGet("cover/{guid}", Name = "GetCoverRoute")]
    public async Task<ActionResult> GetCover(Guid guid)
    {
        var file = await _filesService.GetFileById(guid);
        if (file == null)
            return NotFound();
        var ms = new MemoryStream(file.Data);
        return File(ms, file.Type);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromForm] BookCreationRequest request, IFormFile? file)
    {
        var validationRes = await _bcrValidator.ValidateAsync(request);
        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors);
        }
        string coverLink = string.Empty;
        if(file != null)
        {
            validationRes = await _fileValidator.ValidateAsync(file);
            if (validationRes.IsValid)
            {
                var guid = await _filesService.CreateFile(file.OpenReadStream(), file.ContentType);
                if (guid != Guid.Empty)
                    coverLink = Url.Link("GetCoverRoute", new { Guid = guid })!;
            }
        }
        var id = await _bookService.CreateBook(request, coverLink);
        await _bookStateService.AddBooks(id, request.NumOfBooks);
        return Ok(id);
    }

    [Authorize(Policy = "Admin")]
    [HttpPut]
    public async Task<ActionResult<Guid>> Put([FromForm] BookEditRequest request, IFormFile? file)
    {
        var validationRes = await _berValidator.ValidateAsync(request);
        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors);
        }
        if(!await _bookService.Any(request.Id))
        {
            return NotFound();
        }
        string coverLink = string.Empty;
        if (file != null)
        {
            validationRes = await _fileValidator.ValidateAsync(file);
            if (validationRes.IsValid)
            {
                var guid = await _filesService.CreateFile(file.OpenReadStream(), file.ContentType);
                if (guid != Guid.Empty)
                    coverLink = Url.Link("GetCoverRoute", new { Guid = guid })!;
            }
        }
        var id = await _bookService.EditBook(request, coverLink);
        return Ok(id);
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var bid = await _bookService.DeleteBook(id);
        if (bid == Guid.Empty)
            return NotFound();
        return Ok(bid);
    }

    [HttpGet("{bookid}/available-num")]
    public async Task<ActionResult<int>> GetNumOfAvailableBooks(Guid bookid)
    {
        var res = await _bookStateService.GetAvailableNumOfBooks(bookid);
        if(res < 0)
        {
            return NoContent();
        }
        return Ok(res);
    }
    
    [HttpGet("title/{title}")]
    public async Task<ActionResult<IEnumerable<BookFullResponse>>> GetByTitle(string title)
    {
        if (title.Length < 1)
            return BadRequest();
        var books = await _bookService.GetByTitle(title);
        return Ok(books);
    }
}

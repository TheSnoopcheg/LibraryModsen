using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts.Author;
using LibraryModsen.Application.Contracts.Book;
using LibraryModsen.Validators.Author;
using LibraryModsen.Validators.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryModsen.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController (
        IAuthorService authorService,
        AuthorCreationRequestValidator acrValidator,
        AuthorEditRequestValidator aerValidator) : ControllerBase
{
    private readonly IAuthorService _authorService = authorService;
    private readonly AuthorCreationRequestValidator _acrValidator = acrValidator;
    private readonly AuthorEditRequestValidator _aerValidator = aerValidator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorFullResponse>>> Get()
    {
        var authors = await _authorService.GetAll();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorFullResponse>> Get(Guid id)
    {
        var author = await _authorService.GetById(id);
        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    [HttpGet("page")]
    public async Task<ActionResult<IEnumerable<AuthorFullResponse>>> Get([FromQuery(Name = "p")] int page, [FromQuery(Name = "s")] int size)
    {
        if(page < 1 || size < 1)
        {
            return BadRequest();
        }
        var authors = await _authorService.GetPage(page - 1, size);
        return Ok(authors);
    }

    [HttpGet("{id}/books")]
    public async Task<ActionResult<IEnumerable<BookFullResponse>>> GetBooks(Guid id)
    {
        if(!await _authorService.Any(id))
        {
            return NotFound("Author doesn't exist");
        }
        var books = await _authorService.GetBooks(id);
        return Ok(books);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromBody] AuthorCreationRequest request)
    {
        var validationRes = await _acrValidator.ValidateAsync(request);
        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors);
        }
        var id = await _authorService.CreateAuthor(request);
        return Ok(id);
    }

    [Authorize(Policy = "Admin")]
    [HttpPut]
    public async Task<ActionResult<Guid>> Put([FromBody] AuthorEditRequest request)
    {
        var validationRes = await _aerValidator.ValidateAsync(request);
        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors);
        }
        var id = await _authorService.EditAuthor(request);
        if (id == Guid.Empty)
            return NotFound();
        return Ok(id);
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var aid = await _authorService.DeleteAuthor(id);
        if(aid == Guid.Empty)
            return NotFound();
        return Ok(aid);
    }
}

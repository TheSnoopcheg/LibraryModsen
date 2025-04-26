using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts.Author;
using LibraryModsen.Application.Contracts.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryModsen.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController (
        IAuthorService authorService) : ControllerBase
{
    private readonly IAuthorService _authorService = authorService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorFullResponse>>> Get(CancellationToken cancelToken = default)
    {
        var authors = await _authorService.GetAll(cancelToken);
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorFullResponse>> Get(Guid id, CancellationToken cancelToken = default)
    {
        var author = await _authorService.GetById(id, cancelToken);
        return Ok(author);
    }

    [HttpGet("page")]
    public async Task<ActionResult<IEnumerable<AuthorFullResponse>>> Get([FromQuery(Name = "p")] int page, [FromQuery(Name = "s")] int size, CancellationToken cancelToken = default)
    {
        var authors = await _authorService.GetPage(page, size, cancelToken);
        return Ok(authors);
    }

    [HttpGet("{id}/books")]
    public async Task<ActionResult<IEnumerable<BookFullResponse>>> GetBooks(Guid id, CancellationToken cancelToken = default)
    {
        var books = await _authorService.GetBooks(id, cancelToken);
        return Ok(books);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AuthorCreationRequest request, CancellationToken cancelToken = default)
    {
        await _authorService.CreateAuthor(request, cancelToken);
        return Ok();
    }

    [Authorize(Policy = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] AuthorEditRequest request, CancellationToken cancelToken = default)
    {
        await _authorService.EditAuthor(request, cancelToken);
        return Ok();
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancelToken = default)
    {
        await _authorService.DeleteAuthor(id, cancelToken);
        return Ok();
    }
}

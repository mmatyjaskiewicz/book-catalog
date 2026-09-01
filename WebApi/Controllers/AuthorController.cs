using Application.DTOs.Queries;
using Application.DTOs.Requests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController(AuthorService authorService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(CreateAuthorRequest request)
    {
        await authorService.CreateAsync(request);

        return Created();
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] AuthorQueryParameters queryParameters)
    {
        var authors = await authorService.GetAllAsync(queryParameters);

        return Ok(authors);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var author = await authorService.GetByIdAsync(id);

        return Ok(author);
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateAuthorRequest request)
    {
        var author = await authorService.UpdateAsync(id, request);

        return Ok(author);
    }
}
using Application.DTOs.Queries;
using Application.DTOs.Requests;
using Application.Exceptions.BadRequest;
using Application.Exceptions.NotFound;
using Application.Interfaces.Repositories;
using Application.Models;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AuthorService(IAuthorRepository authorRepository, ILogger<AuthorService> logger)
{
    public async Task<Author> CreateAsync(CreateAuthorRequest request)
    {
        var author = new Author(request.Name);

        await authorRepository.AddAsync(author);

        logger.LogInformation("Author {AuthorId} was created.", author.Id);

        return author;
    }

    public async Task<PagedResult<Author>> GetAllAsync(AuthorQueryParameters queryParameters)
    {
        var result = await authorRepository.GetAllAsync(queryParameters);

        if (result.Items.Count == 0)
        {
            logger.LogWarning("No authors were found.");
            throw new NotFoundException("No authors found.");
        }

        var totalPages = (int)Math.Ceiling((double)result.TotalCount / queryParameters.PageSize);

        if (queryParameters.PageNumber > totalPages)
        {
            logger.LogWarning("Page {PageNumber} is out of range. Total pages: {TotalPages}.", queryParameters.PageNumber, totalPages);

            throw new BadRequestException($"Page number {queryParameters.PageNumber} is out of range. Total pages: {totalPages}.");
        }

        return result;
    }

    public async Task<Author?> GetByIdAsync(Guid id)
    {
        var author = await authorRepository.GetByIdAsync(id);

        if (author == null)
        {
            logger.LogWarning("Author {AuthorId} was not found.", id);
            throw new NotFoundException("Author not found.");
        }

        return author;
    }

    public async Task<Author> UpdateAsync(Guid id, UpdateAuthorRequest request)
    {
        if (request.Name == null)
        {
            throw new BadRequestException("At least one field must be provided.");
        }

        var author = await authorRepository.GetByIdAsync(id);

        if (author == null)
        {
            logger.LogWarning("Author {AuthorId} was not found.", id);
            throw new NotFoundException("Author not found.");
        }

        author.Update(request.Name);

        await authorRepository.UpdateAsync(author);

        logger.LogInformation("Author {AuthorId} was updated.", id);

        return author;
    }
}
using Application.DTOs.Queries;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Exceptions.BadRequest;
using Application.Exceptions.Conflict;
using Application.Exceptions.NotFound;
using Application.Interfaces.Repositories;
using Application.Models;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, ILoanRepository loanRepository, ILogger<BookService> logger)
{
    public async Task<Book> CreateAsync(CreateBookRequest request)
    {
        var author = await authorRepository.GetByIdAsync(request.AuthorId);
        if(author == null)
        {
            logger.LogWarning("Author {AuthorId} was not found.", request.AuthorId);
            throw new NotFoundException("Author not found.");
        }
        
        var book = new Book(request.Title, request.AuthorId, request.PublishYear);
        
        await bookRepository.AddAsync(book);
        logger.LogInformation("Book {BookId} was created.", book.Id);
        
        return book;
    }
    
    public async Task<PagedResult<BookResponse>> GetAllAsync(BookQueryParameters queryParameters)
    {
        var result = await bookRepository.GetAllAsync(queryParameters);

        if (result.Items.Count == 0)
        {
            logger.LogWarning("No books were found.");
            throw new NotFoundException("No books found.");
        }

        var totalPages = (int)Math.Ceiling((double)result.TotalCount / queryParameters.PageSize);

        if (queryParameters.PageNumber > totalPages)
        {
            logger.LogWarning("Page {PageNumber} is out of range. Total pages: {TotalPages}.", queryParameters.PageNumber, totalPages);

            throw new BadRequestException($"Page number {queryParameters.PageNumber} is out of range. Total pages: {totalPages}.");
        }

        var responseItems = new List<BookResponse>();

        foreach (var book in result.Items)
        {
            responseItems.Add(new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                PublishYear = book.PublishYear,
                AuthorId = book.AuthorId
            });
        }

        return new PagedResult<BookResponse>
        {
            Items = responseItems,
            TotalCount = result.TotalCount
        };
    }
    
    public async Task<Book?> GetByIdAsync(Guid id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        if (book == null)
        {
            logger.LogWarning("Book {BookId} was not found.", id);
            throw new NotFoundException("Book not found.");
        }
        
        return book;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        if (book == null)
        {
            logger.LogWarning("Book {BookId} was not found.", id);
            throw new NotFoundException("Book not found.");
        }
        
        var activeLoan = await loanRepository.GetActiveLoanByBookIdAsync(id);
        if (activeLoan != null)
        {
            logger.LogWarning("Book {BookId} is currently on loan and cannot be deleted.", id);
            throw new ConflictException("Book is currently on loan and cannot be deleted.");
        }
        
        await bookRepository.DeleteAsync(book);
        logger.LogInformation("Book {BookId} was deleted.", id);
    }
    
    public async Task<Book> UpdateAsync(Guid id, UpdateBookRequest request)
    {
        if(request.Title == null && request.AuthorId == null && request.PublishYear == null)
        {
            throw new BadRequestException("At least one field must be provided.");
        }
        
        var book = await bookRepository.GetByIdAsync(id);
        if (book == null)
        {
            logger.LogWarning("Book {BookId} was not found.", id);
            throw new NotFoundException("Book not found.");
        }
        
        if(request.Title != null)
        {
            book.UpdateTitle(request.Title);
        }
        
        if (request.AuthorId.HasValue)
        {
            book.UpdateAuthorId(request.AuthorId.Value);
        }
        
        if(request.PublishYear != null)
        {
            book.UpdatePublishYear(request.PublishYear.Value);
        }
        
        await bookRepository.UpdateAsync(book);
        logger.LogInformation("Book {BookId} was updated.", id);
        
        return book;
    }
}
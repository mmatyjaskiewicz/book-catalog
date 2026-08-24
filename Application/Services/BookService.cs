using Application.DTOs.Queries;
using Application.DTOs.Requests;
using Application.Exceptions.BadRequest;
using Application.Exceptions.NotFound;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class BookService(IBookRepository bookRepository, ILogger<BookService> logger)
{
    public async Task<Book> CreateAsync(CreateBookRequest request)
    {
        var book = new Book(request.Title, request.Author, request.Year);
        
        await bookRepository.AddAsync(book);
        logger.LogInformation("Book {BookId} was created.", book.Id);
        
        return book;
    }
    
    public async Task<PagedResult<Book>> GetAllAsync(BookQueryParameters queryParameters)
    {
        var result = await bookRepository.GetAllAsync(queryParameters);
        
        if (result.Items.Count == 0)
        {
            logger.LogWarning("No books were found.");
            throw new NotFoundException("No books found.");
        }
        
        var totalPages = (int)Math.Ceiling((double)result.TotalCount / queryParameters.PageSize);
        
        if(queryParameters.PageNumber > totalPages)
        {
            logger.LogWarning("Page {PageNumber} is out of range. Total pages: {TotalPages}.", queryParameters.PageNumber, totalPages);
            throw new BadRequestException($"Page number {queryParameters.PageNumber} is out of range. Total pages: {totalPages}.");
        }
        
        return result;
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
        
        await bookRepository.DeleteAsync(book);
        logger.LogInformation("Book {BookId} was deleted.", id);
    }
    
    public async Task<Book> UpdateAsync(Guid id, UpdateBookRequest request)
    {
        if(request.Title == null && request.Author == null && request.Year == null)
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
        
        if(request.Author != null)
        {
            book.UpdateAuthor(request.Author);
        }
        
        if(request.Year != null)
        {
            book.UpdateYear(request.Year.Value);
        }
        
        await bookRepository.UpdateAsync(book);
        logger.LogInformation("Book {BookId} was updated.", id);
        
        return book;
    }
}
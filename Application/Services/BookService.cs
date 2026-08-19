using Application.DTOs.Requests;
using Application.Exceptions.BadRequest;
using Application.Exceptions.NotFound;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class BookService(IBookRepository bookRepository)
{
    public async Task<Book> CreateAsync(CreateBookRequest request)
    {
        var book = new Book(request.Title, request.Author, request.Year);
        
        await bookRepository.AddAsync(book);
        return book;
    }
    
    public async Task<List<Book>> GetAllAsync()
    {
        return await bookRepository.GetAllAsync();
    }
    
    public async Task<Book?> GetByIdAsync(Guid id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        
        if (book == null)
        {
            throw new NotFoundException("Book not found.");
        }
        
        return book;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        
        if (book == null)
        {
            throw new NotFoundException("Book not found.");
        }
        
        await bookRepository.DeleteAsync(book);
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
        return book;
    }
}
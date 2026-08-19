using Application.DTOs.Requests;
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
}
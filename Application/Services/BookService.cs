using Application.DTOs.Requests;
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
        return await bookRepository.GetByIdAsync(id);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await bookRepository.DeleteAsync(id);
    }
}
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class FakeBookRepository(ILogger<FakeBookRepository> logger) : IBookRepository
{
    private readonly List<Book> _books = new();
    
    public Task AddAsync(Book book)
    {
        _books.Add(book);
        logger.LogInformation("Book {BookId} was added to the repository.", book.Id);
        return Task.CompletedTask;
    }
    
    public Task<List<Book>> GetAllAsync()
    {
        return Task.FromResult(_books);
    }
    
    public Task<Book?> GetByIdAsync(Guid id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }
    
    public Task DeleteAsync(Book book)
    {
        _books.Remove(book);
        return Task.CompletedTask;
    }
    
    public Task UpdateAsync(Book book)
    {
        return Task.CompletedTask;
    }
}
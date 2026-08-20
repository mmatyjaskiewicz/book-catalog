using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class FakeBookRepository : IBookRepository
{
    private readonly List<Book> _books = new();
    
    public Task AddAsync(Book book)
    {
        _books.Add(book);
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
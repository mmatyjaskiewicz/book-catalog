using Application.DTOs.Queries;
using Application.Interfaces;
using Application.Models;
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
    
    public Task<PagedResult<Book>> GetAllAsync(BookQueryParameters queryParameters)
    {
        var filteredBooks = _books.AsEnumerable();
        
        if (!string.IsNullOrWhiteSpace(queryParameters.Title))
        {
            filteredBooks = filteredBooks.Where(b => b.Title.Contains(queryParameters.Title, StringComparison.OrdinalIgnoreCase));
        }
        
        if (!string.IsNullOrWhiteSpace(queryParameters.Author))
        {
            filteredBooks = filteredBooks.Where(b => b.Author.Contains(queryParameters.Author, StringComparison.OrdinalIgnoreCase));
        }
        
        if (queryParameters.Year.HasValue)
        {
            filteredBooks = filteredBooks.Where(b => b.Year == queryParameters.Year.Value);
        }
        
        var totalCount = filteredBooks.Count();

        var pagedBooks = filteredBooks
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToList();

        var result = new PagedResult<Book>
        {
            Items = pagedBooks,
            TotalCount = totalCount
        };

        return Task.FromResult(result);
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
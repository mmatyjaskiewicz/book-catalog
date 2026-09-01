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
    
    public Task UpdateAsync(Book book)
    {
        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(Book book)
    {
        _books.Remove(book);
        return Task.CompletedTask;
    }
    
    public Task<Book?> GetByIdAsync(Guid id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }
    
    public Task<PagedResult<Book>> GetAllAsync(BookQueryParameters queryParameters)
    {
        var filteredBooks = _books.AsEnumerable();
        
        if (!string.IsNullOrWhiteSpace(queryParameters.Title))
        {
            filteredBooks = filteredBooks.Where(b => b.Title.Contains(queryParameters.Title, StringComparison.OrdinalIgnoreCase));
        }
        
        if (queryParameters.AuthorId.HasValue)
        {
            filteredBooks = filteredBooks.Where(b => b.AuthorId == queryParameters.AuthorId.Value);
        }
        
        if (queryParameters.PublishYear.HasValue)
        {
            filteredBooks = filteredBooks.Where(b => b.PublishYear == queryParameters.PublishYear.Value);
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
}
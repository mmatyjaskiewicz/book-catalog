using Application.DTOs.Queries;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfBookRepository(BookCatalogDbContext context) : IBookRepository
{
    public Task AddAsync(Book book)
    {
        context.Books.Add(book);
        return context.SaveChangesAsync();
    }
    
    public async Task<PagedResult<Book>> GetAllAsync(BookQueryParameters queryParameters)
    {
        var query = context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Title))
        {
            query = query.Where(b => b.Title.Contains(queryParameters.Title));
        }

        if (!string.IsNullOrWhiteSpace(queryParameters.Author))
        {
            query = query.Where(b => b.Author.Contains(queryParameters.Author));
        }

        if (queryParameters.Year.HasValue)
        {
            query = query.Where(b => b.Year == queryParameters.Year.Value);
        }

        var totalCount = await query.CountAsync();

        var pagedBooks = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        return new PagedResult<Book>
        {
            Items = pagedBooks,
            TotalCount = totalCount
        };
    }
    
    public Task<Book?> GetByIdAsync(Guid id)
    {
        return context.Books.FirstOrDefaultAsync(b => b.Id == id);
    }
    
    public Task DeleteAsync(Book book)
    {
        context.Books.Remove(book);
        return context.SaveChangesAsync();
    }
    
    public Task UpdateAsync(Book book)
    {
        context.Books.Update(book);
        return context.SaveChangesAsync();
    }
}
using Application.DTOs.Queries;
using Application.Interfaces.Repositories;
using Application.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EntityFramework;

public class EfBookRepository : EfRepository<Book>, IBookRepository
{ 
    private readonly BookCatalogDbContext _context;

    public EfBookRepository(BookCatalogDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<PagedResult<Book>> GetAllAsync(BookQueryParameters queryParameters)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Title))
        {
            query = query.Where(b => b.Title.Contains(queryParameters.Title));
        }
        
        if (queryParameters.AuthorId.HasValue)
        {
            query = query.Where(b => b.AuthorId == queryParameters.AuthorId.Value);
        }

        if (queryParameters.PublishYear.HasValue)
        {
            query = query.Where(b => b.PublishYear == queryParameters.PublishYear.Value);
        }

        var totalCount = await query.CountAsync();

        var pagedBooks = await query
            .OrderBy(b => b.Title)
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        return new PagedResult<Book>
        {
            Items = pagedBooks,
            TotalCount = totalCount
        };
    }
}
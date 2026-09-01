using Application.DTOs.Queries;
using Application.Interfaces.Repositories;
using Application.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EntityFramework;

public class EfAuthorRepository : EfRepository<Author>, IAuthorRepository
{
    private readonly BookCatalogDbContext _context;

    public EfAuthorRepository(BookCatalogDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedResult<Author>> GetAllAsync(AuthorQueryParameters queryParameters)
    {
        var query = _context.Authors.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Name))
        {
            query = query.Where(a => a.Name.Contains(queryParameters.Name));
        }

        var totalCount = await query.CountAsync();

        var pagedAuthors = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        return new PagedResult<Author>
        {
            Items = pagedAuthors,
            TotalCount = totalCount
        };
    }
}
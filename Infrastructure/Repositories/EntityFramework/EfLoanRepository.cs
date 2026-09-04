using Application.DTOs.Queries;
using Application.Interfaces.Repositories;
using Application.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EntityFramework;

public class EfLoanRepository : EfRepository<Loan>, ILoanRepository
{
    private readonly BookCatalogDbContext _context;

    public EfLoanRepository(BookCatalogDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Loan?> GetActiveLoanByBookIdAsync(Guid bookId)
    {
        return await _context.Loans.FirstOrDefaultAsync(l => l.BookId == bookId && l.ReturnedAt == null);
    }
    
    public async Task<PagedResult<Loan>> GetAllAsync(LoanQueryParameters queryParameters)
    {
        var query = _context.Loans.AsQueryable();

        if (queryParameters.UserId.HasValue)
        {
            query = query.Where(l => l.UserId == queryParameters.UserId.Value);
        }

        if (queryParameters.BookId.HasValue)
        {
            query = query.Where(l => l.BookId == queryParameters.BookId.Value);
        }

        if (queryParameters.ActiveOnly == true)
        {
            query = query.Where(l => l.ReturnedAt == null);
        }
        else if (queryParameters.ActiveOnly == false)
        {
            query = query.Where(l => l.ReturnedAt != null);
        }

        var totalCount = await query.CountAsync();

        var pagedLoans = await query
            .OrderByDescending(l => l.BorrowedAt)
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        return new PagedResult<Loan>
        {
            Items = pagedLoans,
            TotalCount = totalCount
        };
    }
}
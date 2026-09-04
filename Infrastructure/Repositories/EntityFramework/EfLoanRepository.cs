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

   // Active loans
public async Task<Loan?> GetActiveLoanByBookIdAsync(Guid bookId)
{
    return await _context.Loans.FirstOrDefaultAsync(l => l.BookId == bookId);
}

public async Task<PagedResult<Loan>> GetActiveLoansAsync(LoanQueryParameters queryParameters)
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

// Archived loans
public async Task<PagedResult<ArchivedLoan>> GetArchivedLoansAsync(LoanQueryParameters queryParameters)
{
    var query = _context.ArchivedLoans.AsQueryable();

    if (queryParameters.UserId.HasValue)
    {
        query = query.Where(l => l.UserId == queryParameters.UserId.Value);
    }

    if (queryParameters.BookId.HasValue)
    {
        query = query.Where(l => l.BookId == queryParameters.BookId.Value);
    }

    var totalCount = await query.CountAsync();

    var pagedLoans = await query
        .OrderByDescending(l => l.ReturnedAt)
        .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
        .Take(queryParameters.PageSize)
        .ToListAsync();

    return new PagedResult<ArchivedLoan>
    {
        Items = pagedLoans,
        TotalCount = totalCount
    };
}

public async Task ArchiveLoanAsync(Loan loan)
{
    var archivedLoan = new ArchivedLoan(loan.BookId, loan.UserId, loan.BorrowedAt, DateTime.UtcNow);

    _context.Loans.Remove(loan);
    _context.ArchivedLoans.Add(archivedLoan);

    await _context.SaveChangesAsync();
}
}
using Application.Interfaces.Repositories;
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
}
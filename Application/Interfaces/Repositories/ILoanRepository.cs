using Application.DTOs.Queries;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ILoanRepository : IRepository<Loan>
{
    // Active loans
    Task<Loan?> GetActiveLoanByBookIdAsync(Guid bookId);
    Task<PagedResult<Loan>> GetActiveLoansAsync(LoanQueryParameters queryParameters);

    // Archived loans
    Task<PagedResult<ArchivedLoan>> GetArchivedLoansAsync(LoanQueryParameters queryParameters);
    Task ArchiveLoanAsync(Loan loan);
}
using Application.DTOs.Queries;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ILoanRepository : IRepository<Loan>
{
    public Task<Loan?> GetActiveLoanByBookIdAsync(Guid bookId);
    public Task<PagedResult<Loan>> GetActiveLoansAsync(LoanQueryParameters queryParameters);
    public Task<PagedResult<ArchivedLoan>> GetArchivedLoansAsync(LoanQueryParameters queryParameters);
    public Task ArchiveLoanAsync(Loan loan);
}
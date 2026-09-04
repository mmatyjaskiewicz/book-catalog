using Application.DTOs.Queries;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ILoanRepository : IRepository<Loan>
{
    Task<Loan?> GetActiveLoanByBookIdAsync(Guid bookId);
    Task<PagedResult<Loan>> GetActiveLoansAsync(LoanQueryParameters queryParameters);
    Task<PagedResult<ArchivedLoan>> GetArchivedLoansAsync(LoanQueryParameters queryParameters);
}
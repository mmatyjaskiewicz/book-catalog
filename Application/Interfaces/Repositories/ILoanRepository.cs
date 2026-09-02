using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ILoanRepository : IRepository<Loan>
{
    Task<Loan?> GetActiveLoanByBookIdAsync(Guid bookId);
}
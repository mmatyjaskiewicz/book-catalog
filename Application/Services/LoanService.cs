using Application.DTOs.Requests;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services;

public class LoanService(ILoanRepository loanRepository, IBookRepository bookRepository, IUserRepository userRepository)
{
    public async Task BorrowAsync(CreateLoanRequest request)
    {
        var loan = await loanRepository.GetActiveLoanByBookIdAsync(request.BookId);
        if (loan != null)
        {
            throw new Exception("Book is already borrowed.");
        }
        
        var book = await bookRepository.GetByIdAsync(request.BookId);
        if (book == null)
        {
            throw new Exception("Book not found.");
        }
        
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new Exception("User not found.");
        }
        
        var newLoan = new Loan(request.BookId, request.UserId);
        await loanRepository.AddAsync(newLoan);
    }
    
    public async Task ReturnAsync(Guid loanId)
    {
        var loan = await loanRepository.GetByIdAsync(loanId);
        if (loan == null)
        {
            throw new Exception("Loan not found.");
        }
        
        if (loan.ReturnedAt != null)
        {
            throw new Exception("Book has already been returned.");
        }
        
        loan.Return();
        await loanRepository.UpdateAsync(loan);
    }
}
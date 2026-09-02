using Application.DTOs.Queries;
using Application.DTOs.Requests;
using Application.Exceptions.BadRequest;
using Application.Exceptions.Conflict;
using Application.Exceptions.NotFound;
using Application.Interfaces.Repositories;
using Application.Models;
using Domain.Entities;

namespace Application.Services;

public class LoanService(ILoanRepository loanRepository, IBookRepository bookRepository, IUserRepository userRepository)
{
    public async Task BorrowAsync(CreateLoanRequest request)
    {
        var loan = await loanRepository.GetActiveLoanByBookIdAsync(request.BookId);
        if (loan != null)
        {
            throw new ConflictException("Book is already borrowed.");
        }
        
        var book = await bookRepository.GetByIdAsync(request.BookId);
        if (book == null)
        {
            throw new NotFoundException("Book not found.");
        }
        
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }
        
        var newLoan = new Loan(request.BookId, request.UserId);
        await loanRepository.AddAsync(newLoan);
    }
    
    public async Task ReturnAsync(Guid loanId)
    {
        var loan = await loanRepository.GetByIdAsync(loanId);
        if (loan == null)
        {
            throw new NotFoundException("Loan not found.");
        }
        
        if (loan.ReturnedAt != null)
        {
            throw new ConflictException("Book has already been returned.");
        }
        
        loan.Return();
        await loanRepository.UpdateAsync(loan);
    }
    
    public async Task<Loan> GetByIdAsync(Guid loanId)
    {
        var loan = await loanRepository.GetByIdAsync(loanId);
        if (loan == null)
        {
            throw new NotFoundException("Loan not found.");
        }
        
        return loan;
    }

    public async Task<PagedResult<Loan>> GetAllAsync(LoanQueryParameters queryParameters)
    {
        var result = await loanRepository.GetAllAsync(queryParameters);

        if (result.Items.Count == 0)
        {
            throw new NotFoundException("No loans found.");
        }

        var totalPages = (int)Math.Ceiling((double)result.TotalCount / queryParameters.PageSize);

        if (queryParameters.PageNumber > totalPages)
        {
            throw new BadRequestException($"Page number {queryParameters.PageNumber} is out of range. Total pages: {totalPages}.");
        }
        
        return result;
    }
}
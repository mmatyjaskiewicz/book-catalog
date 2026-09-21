using Application.DTOs.Queries;
using Application.DTOs.Requests;
using Application.Exceptions.BadRequest;
using Application.Exceptions.Conflict;
using Application.Exceptions.NotFound;
using Application.Interfaces.Repositories;
using Application.Models;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class LoanService(ILoanRepository loanRepository, IBookRepository bookRepository, IUserRepository userRepository, ILogger<LoanService> logger)
{
    public async Task BorrowAsync(CreateLoanRequest request)
    {
        logger.LogInformation("Attempting to borrow book with ID {BookId} for user with ID {UserId}.", request.BookId, request.UserId);
        
        var loan = await loanRepository.GetActiveLoanByBookIdAsync(request.BookId);
        if (loan != null)
        {
            logger.LogWarning("Book with ID {BookId} is already borrowed.", request.BookId);
            throw new ConflictException("Book is already borrowed.");
        }
        
        var book = await bookRepository.GetByIdAsync(request.BookId);
        if (book == null)
        {
            logger.LogWarning("Book with ID {BookId} was not found.", request.BookId);
            throw new NotFoundException("Book not found.");
        }
        
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            logger.LogWarning("User with ID {UserId} was not found.", request.UserId);
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
            logger.LogWarning("Loan with ID {LoanId} was not found.", loanId);
            throw new NotFoundException("Loan not found.");
        }
        
        await loanRepository.ArchiveLoanAsync(loan);
        logger.LogInformation("Loan with ID {LoanId} was successfully returned.", loanId);
    }
    
    public async Task<Loan> GetByIdAsync(Guid loanId)
    {
        var loan = await loanRepository.GetByIdAsync(loanId);
        if (loan == null)
        {
            logger.LogWarning("Loan with ID {LoanId} was not found.", loanId);
            throw new NotFoundException("Loan not found.");
        }
        
        return loan;
    }

    public async Task<PagedResult<Loan>> GetActiveLoansAsync(LoanQueryParameters queryParameters)
    {
        var result = await loanRepository.GetActiveLoansAsync(queryParameters);
        if (result.Items.Count == 0)
        {
            logger.LogWarning("No active loans found for the given query parameters.");
            throw new NotFoundException("No active loans found.");
        }

        var totalPages = (int)Math.Ceiling((double)result.TotalCount / queryParameters.PageSize);

        if (queryParameters.PageNumber > totalPages)
        {
            logger.LogWarning("Page number {PageNumber} is out of range. Total pages: {TotalPages}.", queryParameters.PageNumber, totalPages);
            throw new BadRequestException($"Page number {queryParameters.PageNumber} is out of range. Total pages: {totalPages}.");
        }
    
        return result;
    }
    
    public async Task<PagedResult<ArchivedLoan>> GetArchivedLoansAsync(LoanQueryParameters queryParameters)
    {
        var result = await loanRepository.GetArchivedLoansAsync(queryParameters);
        if (result.Items.Count == 0)
        {
            logger.LogWarning("No archived loans found for the given query parameters.");
            throw new NotFoundException("No archived loans found.");
        }

        var totalPages = (int)Math.Ceiling((double)result.TotalCount / queryParameters.PageSize);

        if (queryParameters.PageNumber > totalPages)
        {
            logger.LogWarning("Page number {PageNumber} is out of range. Total pages: {TotalPages}.", queryParameters.PageNumber, totalPages);
            throw new BadRequestException($"Page number {queryParameters.PageNumber} is out of range. Total pages: {totalPages}.");
        }

        return result;
    }
}
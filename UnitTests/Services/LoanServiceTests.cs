using Application.DTOs.Requests;
using Application.Interfaces.Repositories;
using Application.Services;
using Domain.Entities;
using Moq;

namespace UnitTests.Services;

public class LoanServiceTests
{
    [Fact]
    public async Task BorrowAsync_WhenCalledConcurrently_ShouldCreateTwoLoans()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var book = new Book("Test Book", Guid.NewGuid(), 2026);
        var user = new User("testuser");

        loanRepository
            .Setup(x => x.GetActiveLoanByBookIdAsync(bookId))
            .ReturnsAsync((Loan?)null);

        bookRepository
            .Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync(book);

        userRepository
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        var service = new LoanService(loanRepository.Object, bookRepository.Object, userRepository.Object);

        var request = new CreateLoanRequest
        {
            BookId = bookId,
            UserId = userId
        };

        // Act
        await Task.WhenAll(service.BorrowAsync(request), service.BorrowAsync(request));

        // Assert
        loanRepository.Verify(x => x.AddAsync(It.IsAny<Loan>()), Times.Exactly(2));
    }
}
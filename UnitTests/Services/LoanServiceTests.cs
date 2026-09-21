using Application.DTOs.Queries;
using Application.DTOs.Requests;
using Application.Exceptions.BadRequest;
using Application.Exceptions.Conflict;
using Application.Exceptions.NotFound;
using Application.Interfaces.Repositories;
using Application.Models;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Services;

public class LoanServiceTests
{
    private readonly Mock<ILoanRepository> _loanRepositoryMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ILogger<LoanService>> _loggerMock = new();

    private LoanService CreateService()
    {
        return new LoanService(
            _loanRepositoryMock.Object,
            _bookRepositoryMock.Object,
            _userRepositoryMock.Object, 
            _loggerMock.Object);
    }

    // Tests for BorrowAsync method in LoanService
    [Fact]
    public async Task BorrowAsync_ShouldCreateLoan_WhenBookIsAvailable()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var book = new Book("Test Book", Guid.NewGuid(), 2026);
        var user = new User("testuser");

        _loanRepositoryMock
            .Setup(x => x.GetActiveLoanByBookIdAsync(bookId))
            .ReturnsAsync((Loan?)null);

        _bookRepositoryMock
            .Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync(book);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        var service = CreateService();

        var request = new CreateLoanRequest
        {
            BookId = bookId,
            UserId = userId
        };

        // Act
        await service.BorrowAsync(request);

        // Assert
        _loanRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Loan>(loan =>
                loan.BookId == bookId &&
                loan.UserId == userId)),
            Times.Once);
    }

    [Fact]
    public async Task BorrowAsync_ShouldThrowConflictException_WhenBookIsAlreadyBorrowed()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var existingLoan = new Loan(bookId, Guid.NewGuid());

        _loanRepositoryMock
            .Setup(x => x.GetActiveLoanByBookIdAsync(bookId))
            .ReturnsAsync(existingLoan);

        var service = CreateService();

        var request = new CreateLoanRequest
        {
            BookId = bookId,
            UserId = userId
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => service.BorrowAsync(request));

        _bookRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);

        _userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);

        _loanRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Loan>()), Times.Never);
    }

    [Fact]
    public async Task BorrowAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _loanRepositoryMock
            .Setup(x => x.GetActiveLoanByBookIdAsync(bookId))
            .ReturnsAsync((Loan?)null);

        _bookRepositoryMock
            .Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync((Book?)null);

        var service = CreateService();

        var request = new CreateLoanRequest
        {
            BookId = bookId,
            UserId = userId
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.BorrowAsync(request));

        _userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);

        _loanRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Loan>()), Times.Never);
    }

    [Fact]
    public async Task BorrowAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var book = new Book("Test Book", Guid.NewGuid(), 2026);

        _loanRepositoryMock
            .Setup(x => x.GetActiveLoanByBookIdAsync(bookId))
            .ReturnsAsync((Loan?)null);

        _bookRepositoryMock
            .Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync(book);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        var service = CreateService();

        var request = new CreateLoanRequest
        {
            BookId = bookId,
            UserId = userId
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.BorrowAsync(request));

        _loanRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Loan>()), Times.Never);
    }

    // Tests for ReturnAsync method in LoanService
    [Fact]
    public async Task ReturnAsync_ShouldArchiveLoan_WhenLoanExists()
    {
        // Arrange
        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid());

        _loanRepositoryMock
            .Setup(x => x.GetByIdAsync(loan.Id))
            .ReturnsAsync(loan);

        var service = CreateService();

        // Act
        await service.ReturnAsync(loan.Id);

        // Assert
        _loanRepositoryMock.Verify(x => x.ArchiveLoanAsync(loan), Times.Once);
    }

    [Fact]
    public async Task ReturnAsync_ShouldThrowNotFoundException_WhenLoanDoesNotExist()
    {
        // Arrange
        var loanId = Guid.NewGuid();

        _loanRepositoryMock
            .Setup(x => x.GetByIdAsync(loanId))
            .ReturnsAsync((Loan?)null);

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.ReturnAsync(loanId));

        _loanRepositoryMock.Verify(x => x.ArchiveLoanAsync(It.IsAny<Loan>()), Times.Never);
    }

    // Tests for GetByIdAsync method in LoanService
    [Fact]
    public async Task GetByIdAsync_ShouldReturnLoan_WhenLoanExists()
    {
        // Arrange
        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid());

        _loanRepositoryMock
            .Setup(x => x.GetByIdAsync(loan.Id))
            .ReturnsAsync(loan);

        var service = CreateService();

        // Act
        var result = await service.GetByIdAsync(loan.Id);

        // Assert
        Assert.Equal(loan, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenLoanDoesNotExist()
    {
        // Arrange
        var loanId = Guid.NewGuid();

        _loanRepositoryMock
            .Setup(x => x.GetByIdAsync(loanId))
            .ReturnsAsync((Loan?)null);

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(loanId));
    }

    // Tests for GetActiveLoansAsync method in LoanService
    [Fact]
    public async Task GetActiveLoansAsync_ShouldReturnLoans_WhenActiveLoansExist()
    {
        // Arrange
        var queryParameters = new LoanQueryParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid());

        var pagedResult = new PagedResult<Loan>
        {
            Items = [loan],
            TotalCount = 1
        };

        _loanRepositoryMock
            .Setup(x => x.GetActiveLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = CreateService();

        // Act
        var result = await service.GetActiveLoansAsync(queryParameters);

        // Assert
        Assert.Equal(pagedResult, result);
    }

    [Fact]
    public async Task GetActiveLoansAsync_ShouldThrowNotFoundException_WhenNoActiveLoansFound()
    {
        // Arrange
        var queryParameters = new LoanQueryParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        var pagedResult = new PagedResult<Loan>
        {
            Items = [],
            TotalCount = 0
        };

        _loanRepositoryMock
            .Setup(x => x.GetActiveLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetActiveLoansAsync(queryParameters));
    }

    [Fact]
    public async Task GetActiveLoansAsync_ShouldThrowBadRequestException_WhenPageNumberIsOutOfRange()
    {
        // Arrange
        var queryParameters = new LoanQueryParameters
        {
            PageNumber = 4,
            PageSize = 10
        };

        var pagedResult = new PagedResult<Loan>
        {
            Items = [new Loan(Guid.NewGuid(), Guid.NewGuid())],
            TotalCount = 25
        };

        _loanRepositoryMock
            .Setup(x => x.GetActiveLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => service.GetActiveLoansAsync(queryParameters));
    }

    // Tests for GetArchivedLoansAsync method in LoanService
    [Fact]
    public async Task GetArchivedLoansAsync_ShouldReturnLoans_WhenArchivedLoansExist()
    {
        // Arrange
        var queryParameters = new LoanQueryParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        var archivedLoan = new ArchivedLoan(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(-5),
            DateTime.UtcNow);

        var pagedResult = new PagedResult<ArchivedLoan>
        {
            Items = [archivedLoan],
            TotalCount = 1
        };

        _loanRepositoryMock
            .Setup(x => x.GetArchivedLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = CreateService();

        // Act
        var result = await service.GetArchivedLoansAsync(queryParameters);

        // Assert
        Assert.Equal(pagedResult, result);
    }

    [Fact]
    public async Task GetArchivedLoansAsync_ShouldThrowNotFoundException_WhenNoArchivedLoansFound()
    {
        // Arrange
        var queryParameters = new LoanQueryParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        var pagedResult = new PagedResult<ArchivedLoan>
        {
            Items = [],
            TotalCount = 0
        };

        _loanRepositoryMock
            .Setup(x => x.GetArchivedLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetArchivedLoansAsync(queryParameters));
    }

    [Fact]
    public async Task GetArchivedLoansAsync_ShouldThrowBadRequestException_WhenPageNumberIsOutOfRange()
    {
        // Arrange
        var queryParameters = new LoanQueryParameters
        {
            PageNumber = 4,
            PageSize = 10
        };

        var pagedResult = new PagedResult<ArchivedLoan>
        {
            Items =
            [
                new ArchivedLoan(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.UtcNow.AddDays(-5),
                    DateTime.UtcNow)
            ],
            TotalCount = 25
        };

        _loanRepositoryMock
            .Setup(x => x.GetArchivedLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => service.GetArchivedLoansAsync(queryParameters));
    }
}
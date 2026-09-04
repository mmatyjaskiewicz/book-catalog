using Application.DTOs.Queries;
using Application.DTOs.Requests;
using Application.Exceptions.BadRequest;
using Application.Exceptions.Conflict;
using Application.Exceptions.NotFound;
using Application.Interfaces.Repositories;
using Application.Models;
using Application.Services;
using Domain.Entities;
using Moq;

namespace UnitTests.Services;

public class LoanServiceTests
{
    // Tests for BorrowAsync method in LoanService
    [Fact]
    public async Task BorrowAsync_ShouldCreateLoan_WhenBookIsAvailable()
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

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        var request = new CreateLoanRequest
        {
            BookId = bookId,
            UserId = userId
        };

        // Act
        await service.BorrowAsync(request);

        // Assert
        loanRepository.Verify(
            x => x.AddAsync(It.Is<Loan>(loan =>
                loan.BookId == bookId &&
                loan.UserId == userId)),
            Times.Once);
    }

    [Fact]
    public async Task BorrowAsync_ShouldThrowConflictException_WhenBookIsAlreadyBorrowed()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var existingLoan = new Loan(bookId, Guid.NewGuid());

        loanRepository
            .Setup(x => x.GetActiveLoanByBookIdAsync(bookId))
            .ReturnsAsync(existingLoan);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        var request = new CreateLoanRequest
        {
            BookId = bookId,
            UserId = userId
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => service.BorrowAsync(request));

        bookRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        loanRepository.Verify(x => x.AddAsync(It.IsAny<Loan>()), Times.Never);
    }

    [Fact]
    public async Task BorrowAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        loanRepository
            .Setup(x => x.GetActiveLoanByBookIdAsync(bookId))
            .ReturnsAsync((Loan?)null);

        bookRepository
            .Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync((Book?)null);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        var request = new CreateLoanRequest
        {
            BookId = bookId,
            UserId = userId
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.BorrowAsync(request));

        userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        loanRepository.Verify(x => x.AddAsync(It.IsAny<Loan>()), Times.Never);
    }

    [Fact]
    public async Task BorrowAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var book = new Book("Test Book", Guid.NewGuid(), 2026);

        loanRepository
            .Setup(x => x.GetActiveLoanByBookIdAsync(bookId))
            .ReturnsAsync((Loan?)null);

        bookRepository
            .Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync(book);

        userRepository
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        var request = new CreateLoanRequest
        {
            BookId = bookId,
            UserId = userId
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.BorrowAsync(request));

        loanRepository.Verify(x => x.AddAsync(It.IsAny<Loan>()), Times.Never);
    }

    // Tests for ReturnAsync method in LoanService
    [Fact]
    public async Task ReturnAsync_ShouldArchiveLoan_WhenLoanExists()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid());

        loanRepository
            .Setup(x => x.GetByIdAsync(loan.Id))
            .ReturnsAsync(loan);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        // Act
        await service.ReturnAsync(loan.Id);

        // Assert
        loanRepository.Verify(x => x.ArchiveLoanAsync(loan), Times.Once);
    }

    [Fact]
    public async Task ReturnAsync_ShouldThrowNotFoundException_WhenLoanDoesNotExist()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

        var loanId = Guid.NewGuid();

        loanRepository
            .Setup(x => x.GetByIdAsync(loanId))
            .ReturnsAsync((Loan?)null);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.ReturnAsync(loanId));

        loanRepository.Verify(x => x.ArchiveLoanAsync(It.IsAny<Loan>()), Times.Never);
    }

    // Tests for GetByIdAsync method in LoanService
    [Fact]
    public async Task GetByIdAsync_ShouldReturnLoan_WhenLoanExists()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid());

        loanRepository
            .Setup(x => x.GetByIdAsync(loan.Id))
            .ReturnsAsync(loan);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        // Act
        var result = await service.GetByIdAsync(loan.Id);

        // Assert
        Assert.Equal(loan, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenLoanDoesNotExist()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

        var loanId = Guid.NewGuid();

        loanRepository
            .Setup(x => x.GetByIdAsync(loanId))
            .ReturnsAsync((Loan?)null);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(loanId));
    }

    // Tests for GetActiveLoansAsync method in LoanService
    [Fact]
    public async Task GetActiveLoansAsync_ShouldReturnLoans_WhenActiveLoansExist()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

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

        loanRepository
            .Setup(x => x.GetActiveLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        // Act
        var result = await service.GetActiveLoansAsync(queryParameters);

        // Assert
        Assert.Equal(pagedResult, result);
    }

    [Fact]
    public async Task GetActiveLoansAsync_ShouldThrowNotFoundException_WhenNoActiveLoansFound()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

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

        loanRepository
            .Setup(x => x.GetActiveLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetActiveLoansAsync(queryParameters));
    }

    [Fact]
    public async Task GetActiveLoansAsync_ShouldThrowBadRequestException_WhenPageNumberIsOutOfRange()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

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

        loanRepository
            .Setup(x => x.GetActiveLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => service.GetActiveLoansAsync(queryParameters));
    }

    // Tests for GetArchivedLoansAsync method in LoanService
    [Fact]
    public async Task GetArchivedLoansAsync_ShouldReturnLoans_WhenArchivedLoansExist()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

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

        loanRepository
            .Setup(x => x.GetArchivedLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        // Act
        var result = await service.GetArchivedLoansAsync(queryParameters);

        // Assert
        Assert.Equal(pagedResult, result);
    }

    [Fact]
    public async Task GetArchivedLoansAsync_ShouldThrowNotFoundException_WhenNoArchivedLoansFound()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

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

        loanRepository
            .Setup(x => x.GetArchivedLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetArchivedLoansAsync(queryParameters));
    }

    [Fact]
    public async Task GetArchivedLoansAsync_ShouldThrowBadRequestException_WhenPageNumberIsOutOfRange()
    {
        // Arrange
        var loanRepository = new Mock<ILoanRepository>();
        var bookRepository = new Mock<IBookRepository>();
        var userRepository = new Mock<IUserRepository>();

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

        loanRepository
            .Setup(x => x.GetArchivedLoansAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var service = new LoanService(
            loanRepository.Object,
            bookRepository.Object,
            userRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => service.GetArchivedLoansAsync(queryParameters));
    }
}
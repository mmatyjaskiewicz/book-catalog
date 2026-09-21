using Application.DTOs.Queries;
using Application.DTOs.Requests;
using Application.Exceptions.BadRequest;
using Application.Exceptions.NotFound;
using Application.Interfaces.Repositories;
using Application.Models;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IAuthorRepository> _authorRepositoryMock = new();
    private readonly Mock<ILoanRepository> _loanRepositoryMock = new();
    private readonly Mock<ILogger<BookService>> _loggerMock = new();

    private BookService CreateService()
    {
        return new BookService(_bookRepositoryMock.Object, _authorRepositoryMock.Object, _loanRepositoryMock.Object, _loggerMock.Object);
    }

    // Tests for GetAllAsync method in BookService
    [Fact]
    public async Task GetAllAsync_ShouldReturnBooks_WhenBooksExist()
    {
        // Arrange
        var queryParameters = new BookQueryParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        var book = new Book("Mock title", Guid.NewGuid(), 2024);

        var pagedResult = new PagedResult<Book>
        {
            Items = [book],
            TotalCount = 1
        };

        _bookRepositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var bookService = CreateService();

        // Act
        var result = await bookService.GetAllAsync(queryParameters);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);

        var returnedBook = result.Items[0];

        Assert.Equal(book.Id, returnedBook.Id);
        Assert.Equal(book.Title, returnedBook.Title);
        Assert.Equal(book.PublishYear, returnedBook.PublishYear);
        Assert.Equal(book.AuthorId, returnedBook.AuthorId);
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowNotFoundException_WhenNoBooksFound()
    {
        // Arrange
        var queryParameters = new BookQueryParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        var pagedResult = new PagedResult<Book>
        {
            Items = [],
            TotalCount = 0
        };

        _bookRepositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var bookService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => bookService.GetAllAsync(queryParameters));
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowBadRequestException_WhenPageNumberIsOutOfRange()
    {
        // Arrange
        var queryParameters = new BookQueryParameters
        {
            PageNumber = 4,
            PageSize = 10
        };

        var pagedResult = new PagedResult<Book>
        {
            Items = [new Book("Mock title", Guid.NewGuid(), 2024)],
            TotalCount = 25
        };

        _bookRepositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var bookService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => bookService.GetAllAsync(queryParameters));
    }

    // Tests for GetByIdAsync method in BookService
    [Fact]
    public async Task GetByIdAsync_ShouldReturnBook_WhenBookExists()
    {
        // Arrange
        var book = new Book("Mock title", Guid.NewGuid(), 2024);

        _bookRepositoryMock
            .Setup(x => x.GetByIdAsync(book.Id))
            .ReturnsAsync(book);

        var bookService = CreateService();

        // Act
        var result = await bookService.GetByIdAsync(book.Id);

        // Assert
        Assert.Equal(book, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
    {
        // Arrange
        var fakeBookId = Guid.NewGuid();

        _bookRepositoryMock
            .Setup(x => x.GetByIdAsync(fakeBookId))
            .ReturnsAsync((Book?)null);

        var bookService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => bookService.GetByIdAsync(fakeBookId));
    }

    // Tests for CreateAsync method in BookService
    [Fact]
    public async Task CreateAsync_ShouldCallRepositoryCreate_WhenCalled()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = new Author("Mock author");

        _authorRepositoryMock
            .Setup(x => x.GetByIdAsync(authorId))
            .ReturnsAsync(author);

        var request = new CreateBookRequest
        {
            Title = "Mock title",
            AuthorId = authorId,
            PublishYear = 2024
        };

        var bookService = CreateService();

        // Act
        await bookService.CreateAsync(request);

        // Assert
        _bookRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Book>(book =>
                book.Title == request.Title &&
                book.AuthorId == request.AuthorId &&
                book.PublishYear == request.PublishYear)),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedBook_WhenCalled()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = new Author("Mock author");

        _authorRepositoryMock
            .Setup(x => x.GetByIdAsync(authorId))
            .ReturnsAsync(author);

        var request = new CreateBookRequest
        {
            Title = "Mock title",
            AuthorId = authorId,
            PublishYear = 2024
        };

        var bookService = CreateService();

        // Act
        var result = await bookService.CreateAsync(request);

        // Assert
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.AuthorId, result.AuthorId);
        Assert.Equal(request.PublishYear, result.PublishYear);
    }

    // Tests for DeleteAsync method in BookService
    [Fact]
    public async Task DeleteAsync_ShouldDeleteBook_WhenBookExists()
    {
        // Arrange
        var book = new Book("Mock title", Guid.NewGuid(), 2024);

        _bookRepositoryMock
            .Setup(x => x.GetByIdAsync(book.Id))
            .ReturnsAsync(book);

        _loanRepositoryMock
            .Setup(x => x.GetActiveLoanByBookIdAsync(book.Id))
            .ReturnsAsync((Loan?)null);

        var bookService = CreateService();

        // Act
        await bookService.DeleteAsync(book.Id);

        // Assert
        _bookRepositoryMock.Verify(x => x.DeleteAsync(book), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
    {
        // Arrange
        var bookId = Guid.NewGuid();

        _bookRepositoryMock
            .Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync((Book?)null);

        var bookService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => bookService.DeleteAsync(bookId));

        _bookRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Book>()), Times.Never);
    }

    // Tests for UpdateAsync method in BookService
    [Fact]
    public async Task UpdateAsync_ShouldUpdateBook_WhenBookExists()
    {
        // Arrange
        var book = new Book("Old title", Guid.NewGuid(), 2000);
        var authorId = Guid.NewGuid();

        var request = new UpdateBookRequest
        {
            Title = "New title",
            AuthorId = authorId,
            PublishYear = 2020
        };

        _bookRepositoryMock
            .Setup(x => x.GetByIdAsync(book.Id))
            .ReturnsAsync(book);

        var bookService = CreateService();

        // Act
        var result = await bookService.UpdateAsync(book.Id, request);

        // Assert
        Assert.Equal("New title", result.Title);
        Assert.Equal(authorId, result.AuthorId);
        Assert.Equal(2020, result.PublishYear);

        _bookRepositoryMock.Verify(x => x.UpdateAsync(book), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateOnlyProvidedFields()
    {
        // Arrange
        var originalAuthorId = Guid.NewGuid();
        var book = new Book("Old title", originalAuthorId, 2000);

        var request = new UpdateBookRequest
        {
            Title = "New title"
        };

        _bookRepositoryMock
            .Setup(x => x.GetByIdAsync(book.Id))
            .ReturnsAsync(book);

        var bookService = CreateService();

        // Act
        var result = await bookService.UpdateAsync(book.Id, request);

        // Assert
        Assert.Equal("New title", result.Title);
        Assert.Equal(originalAuthorId, result.AuthorId);
        Assert.Equal(2000, result.PublishYear);

        _bookRepositoryMock.Verify(x => x.UpdateAsync(book), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowBadRequestException_WhenNoFieldsAreProvided()
    {
        // Arrange
        var request = new UpdateBookRequest();
        var bookService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => bookService.UpdateAsync(Guid.NewGuid(), request));

        _bookRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);

        _bookRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Book>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
    {
        // Arrange
        var bookId = Guid.NewGuid();

        var request = new UpdateBookRequest
        {
            Title = "New title",
            AuthorId = Guid.NewGuid(),
            PublishYear = 2020
        };

        _bookRepositoryMock
            .Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync((Book?)null);

        var bookService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => bookService.UpdateAsync(bookId, request));
    }
}
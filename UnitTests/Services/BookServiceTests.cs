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
    // Tests for GetAllAsync method in BookService
    [Fact]
    public async Task GetAllAsync_ShouldReturnBooks_WhenBooksExist()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

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

        repositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var bookService = new BookService(repositoryMock.Object, loggerMock.Object);

        // Act
        var result = await bookService.GetAllAsync(queryParameters);

        // Assert
        Assert.Equal(pagedResult, result);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldThrowNotFoundException_WhenNoBooksFound()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

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

        repositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var bookService = new BookService(repositoryMock.Object, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => bookService.GetAllAsync(queryParameters));
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldThrowBadRequestException_WhenPageNumberIsOutOfRange()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        var queryParameters = new BookQueryParameters
        {
            PageNumber = 4,
            PageSize = 10
        };

        var pagedResult = new PagedResult<Book>
        {
            Items =
            [
                new Book("Mock title", Guid.NewGuid(), 2024)
            ],
            TotalCount = 25
        };

        repositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var bookService = new BookService(repositoryMock.Object, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => bookService.GetAllAsync(queryParameters));
    }
    
    // Tests for GetByIdAsync method in BookService
    [Fact]
    public async Task GetByIdAsync_ShouldReturnBook_WhenBookExists()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();
        
        var book = new Book("Mock title", Guid.NewGuid(), 2024);
        
        repositoryMock
            .Setup(x => x.GetByIdAsync(book.Id))
            .ReturnsAsync(book);
        
        var bookService = new BookService(repositoryMock.Object, loggerMock.Object);
        
        // Act
        var result = await bookService.GetByIdAsync(book.Id);
        
        // Assert
        Assert.Equal(book, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();
        
        var fakeBookId = Guid.NewGuid();
        
        repositoryMock
            .Setup(x => x.GetByIdAsync(fakeBookId))
            .ReturnsAsync((Book)null!);
        
        var bookService = new BookService(repositoryMock.Object, loggerMock.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => bookService.GetByIdAsync(fakeBookId));
    }
    
    // Tests for CreateAsync method in BookService
    [Fact]
    public async Task CreateAsync_ShouldCallRepositoryCreate_WhenCalled()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();
        
        var bookService = new BookService(repositoryMock.Object, loggerMock.Object);
        
        var request = new CreateBookRequest
        {
            Title = "Mock title",
            AuthorId = Guid.NewGuid(),
            PublishYear = 2024
        };
        
        await bookService.CreateAsync(request);
        
        repositoryMock.Verify(
            x => x.AddAsync(It.Is<Book>(book =>
                book.Title == request.Title &&
                book.AuthorId == request.AuthorId &&
                book.PublishYear == request.PublishYear)),
            Times.Once);
    }
    
    // Tests for DeleteAsync method in BookService
    [Fact]
    public async Task DeleteAsync_ShouldDeleteBook_WhenBookExists()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        var book = new Book("Mock title", Guid.NewGuid(), 2024);

        repositoryMock
            .Setup(x => x.GetByIdAsync(book.Id))
            .ReturnsAsync(book);

        var bookService = new BookService(repositoryMock.Object, loggerMock.Object);

        // Act
        await bookService.DeleteAsync(book.Id);

        // Assert
        repositoryMock.Verify(x => x.DeleteAsync(book), Times.Once);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        var bookId = Guid.NewGuid();

        repositoryMock
            .Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync((Book)null!);

        var bookService = new BookService(repositoryMock.Object, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => bookService.DeleteAsync(bookId));
    }
    
    // Tests for UpdateAsync method in BookService
    [Fact]
    public async Task UpdateAsync_ShouldUpdateBook_WhenBookExists()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        var book = new Book("Old title", Guid.NewGuid(), 2000);
        var authorId = Guid.NewGuid();

        var request = new UpdateBookRequest
        {
            Title = "New title",
            AuthorId = authorId,
            PublishYear = 2020
        };

        repositoryMock
            .Setup(x => x.GetByIdAsync(book.Id))
            .ReturnsAsync(book);

        var bookService = new BookService(repositoryMock.Object, loggerMock.Object);

        // Act
        var result = await bookService.UpdateAsync(book.Id, request);

        // Assert
        Assert.Equal("New title", result.Title);
        Assert.Equal(authorId, result.AuthorId);
        Assert.Equal(2020, result.PublishYear);

        repositoryMock.Verify(x => x.UpdateAsync(book), Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        var bookId = Guid.NewGuid();

        var request = new UpdateBookRequest
        {
            Title = "New title",
            AuthorId = Guid.NewGuid(),
            PublishYear = 2020
        };

        repositoryMock
            .Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync((Book)null!);

        var bookService = new BookService(
            repositoryMock.Object,
            loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => bookService.UpdateAsync(bookId, request));
    }
}
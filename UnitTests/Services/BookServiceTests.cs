using Application.DTOs.Requests;
using Application.Exceptions.NotFound;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Services;

public class BookServiceTests
{
    // Tests for GetAllAsync method in BookService
    // TODO: Add GetAllAsync tests
    
    // Tests for GetByIdAsync method in BookService
    [Fact]
    public async Task GetByIdAsync_ShouldReturnBook_WhenBookExists()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();
        
        var book = new Book("Mock title", "Mock author", 2024);
        
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
            Author = "Mock author",
            Year = 2024
        };
        
        await bookService.CreateAsync(request);
        
        repositoryMock.Verify(
            x => x.AddAsync(It.Is<Book>(book =>
                book.Title == request.Title &&
                book.Author == request.Author &&
                book.Year == request.Year)),
            Times.Once);
    }
    
    // Tests for DeleteAsync method in BookService
    [Fact]
    public async Task DeleteAsync_ShouldDeleteBook_WhenBookExists()
    {
        // Arrange
        var repositoryMock = new Mock<IBookRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        var book = new Book("Mock title", "Mock author", 2024);

        repositoryMock
            .Setup(x => x.GetByIdAsync(book.Id))
            .ReturnsAsync(book);

        var bookService = new BookService(repositoryMock.Object, loggerMock.Object);

        // Act
        await bookService.DeleteAsync(book.Id);

        // Assert
        repositoryMock.Verify(
            x => x.DeleteAsync(book),
            Times.Once);
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
}
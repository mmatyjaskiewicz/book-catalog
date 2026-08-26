using Application.Exceptions.NotFound;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Services;

public class BookServiceTests
{
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
}
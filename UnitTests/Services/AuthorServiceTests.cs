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

public class AuthorServiceTests
{
    // Tests for CreateAsync method in AuthorService
    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedAuthor_WhenCalled()
    {
        // Arrange
        var repositoryMock = new Mock<IAuthorRepository>();
        var loggerMock = new Mock<ILogger<AuthorService>>();

        var authorService = new AuthorService(repositoryMock.Object, loggerMock.Object);

        var request = new CreateAuthorRequest
        {
            Name = "Mock author"
        };

        // Act
        var result = await authorService.CreateAsync(request);

        // Assert
        Assert.Equal(request.Name, result.Name);

        repositoryMock.Verify(
            x => x.AddAsync(It.Is<Author>(author =>
                author.Name == request.Name)),
            Times.Once);
    }

    // Tests for GetAllAsync method in AuthorService
    [Fact]
    public async Task GetAllAsync_ShouldReturnAuthors_WhenAuthorsExist()
    {
        // Arrange
        var repositoryMock = new Mock<IAuthorRepository>();
        var loggerMock = new Mock<ILogger<AuthorService>>();

        var queryParameters = new AuthorQueryParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        var author = new Author("Mock author");

        var pagedResult = new PagedResult<Author>
        {
            Items = [author],
            TotalCount = 1
        };

        repositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var authorService = new AuthorService(repositoryMock.Object, loggerMock.Object);

        // Act
        var result = await authorService.GetAllAsync(queryParameters);

        // Assert
        Assert.Equal(pagedResult, result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowNotFoundException_WhenNoAuthorsFound()
    {
        // Arrange
        var repositoryMock = new Mock<IAuthorRepository>();
        var loggerMock = new Mock<ILogger<AuthorService>>();

        var queryParameters = new AuthorQueryParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        var pagedResult = new PagedResult<Author>
        {
            Items = [],
            TotalCount = 0
        };

        repositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var authorService = new AuthorService(repositoryMock.Object, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => authorService.GetAllAsync(queryParameters));
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowBadRequestException_WhenPageNumberIsOutOfRange()
    {
        // Arrange
        var repositoryMock = new Mock<IAuthorRepository>();
        var loggerMock = new Mock<ILogger<AuthorService>>();

        var queryParameters = new AuthorQueryParameters
        {
            PageNumber = 4,
            PageSize = 10
        };

        var pagedResult = new PagedResult<Author>
        {
            Items = [new Author("Mock author")],
            TotalCount = 25
        };

        repositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var authorService = new AuthorService(repositoryMock.Object, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => authorService.GetAllAsync(queryParameters));
    }

    // Tests for GetByIdAsync method in AuthorService
    [Fact]
    public async Task GetByIdAsync_ShouldReturnAuthor_WhenAuthorExists()
    {
        // Arrange
        var repositoryMock = new Mock<IAuthorRepository>();
        var loggerMock = new Mock<ILogger<AuthorService>>();

        var author = new Author("Mock author");

        repositoryMock
            .Setup(x => x.GetByIdAsync(author.Id))
            .ReturnsAsync(author);

        var authorService = new AuthorService(repositoryMock.Object, loggerMock.Object);

        // Act
        var result = await authorService.GetByIdAsync(author.Id);

        // Assert
        Assert.Equal(author, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
    {
        // Arrange
        var repositoryMock = new Mock<IAuthorRepository>();
        var loggerMock = new Mock<ILogger<AuthorService>>();

        var authorId = Guid.NewGuid();

        repositoryMock
            .Setup(x => x.GetByIdAsync(authorId))
            .ReturnsAsync((Author)null!);

        var authorService = new AuthorService(repositoryMock.Object, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => authorService.GetByIdAsync(authorId));
    }

    // Tests for UpdateAsync method in AuthorService
    [Fact]
    public async Task UpdateAsync_ShouldUpdateAuthor_WhenAuthorExists()
    {
        // Arrange
        var repositoryMock = new Mock<IAuthorRepository>();
        var loggerMock = new Mock<ILogger<AuthorService>>();

        var author = new Author("Old author");

        var request = new UpdateAuthorRequest
        {
            Name = "New author"
        };

        repositoryMock
            .Setup(x => x.GetByIdAsync(author.Id))
            .ReturnsAsync(author);

        var authorService = new AuthorService(repositoryMock.Object, loggerMock.Object);

        // Act
        var result = await authorService.UpdateAsync(author.Id, request);

        // Assert
        Assert.Equal("New author", result.Name);

        repositoryMock.Verify(x => x.UpdateAsync(author), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowBadRequestException_WhenNameIsNotProvided()
    {
        // Arrange
        var repositoryMock = new Mock<IAuthorRepository>();
        var loggerMock = new Mock<ILogger<AuthorService>>();

        var authorService = new AuthorService(repositoryMock.Object, loggerMock.Object);

        var request = new UpdateAuthorRequest();

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => authorService.UpdateAsync(Guid.NewGuid(), request));

        repositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Author>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
    {
        // Arrange
        var repositoryMock = new Mock<IAuthorRepository>();
        var loggerMock = new Mock<ILogger<AuthorService>>();

        var authorId = Guid.NewGuid();

        var request = new UpdateAuthorRequest
        {
            Name = "New author"
        };

        repositoryMock
            .Setup(x => x.GetByIdAsync(authorId))
            .ReturnsAsync((Author)null!);

        var authorService = new AuthorService(repositoryMock.Object, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => authorService.UpdateAsync(authorId, request));
    }
}
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
    private readonly Mock<IAuthorRepository> _authorRepositoryMock = new();
    private readonly Mock<ILogger<AuthorService>> _loggerMock = new();

    private AuthorService CreateService()
    {
        return new AuthorService(_authorRepositoryMock.Object, _loggerMock.Object);
    }

    // Tests for CreateAsync method in AuthorService
    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedAuthor_WhenCalled()
    {
        // Arrange
        var authorService = CreateService();

        var request = new CreateAuthorRequest
        {
            Name = "Mock author"
        };

        // Act
        var result = await authorService.CreateAsync(request);

        // Assert
        Assert.Equal(request.Name, result.Name);

        _authorRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Author>(author =>
                author.Name == request.Name)),
            Times.Once);
    }

    // Tests for GetAllAsync method in AuthorService
    [Fact]
    public async Task GetAllAsync_ShouldReturnAuthors_WhenAuthorsExist()
    {
        // Arrange
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

        _authorRepositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var authorService = CreateService();

        // Act
        var result = await authorService.GetAllAsync(queryParameters);

        // Assert
        Assert.Equal(pagedResult, result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowNotFoundException_WhenNoAuthorsFound()
    {
        // Arrange
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

        _authorRepositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var authorService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => authorService.GetAllAsync(queryParameters));
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowBadRequestException_WhenPageNumberIsOutOfRange()
    {
        // Arrange
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

        _authorRepositoryMock
            .Setup(x => x.GetAllAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        var authorService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => authorService.GetAllAsync(queryParameters));
    }

    // Tests for GetByIdAsync method in AuthorService
    [Fact]
    public async Task GetByIdAsync_ShouldReturnAuthor_WhenAuthorExists()
    {
        // Arrange
        var author = new Author("Mock author");

        _authorRepositoryMock
            .Setup(x => x.GetByIdAsync(author.Id))
            .ReturnsAsync(author);

        var authorService = CreateService();

        // Act
        var result = await authorService.GetByIdAsync(author.Id);

        // Assert
        Assert.Equal(author, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
    {
        // Arrange
        var authorId = Guid.NewGuid();

        _authorRepositoryMock
            .Setup(x => x.GetByIdAsync(authorId))
            .ReturnsAsync((Author)null!);

        var authorService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => authorService.GetByIdAsync(authorId));
    }

    // Tests for UpdateAsync method in AuthorService
    [Fact]
    public async Task UpdateAsync_ShouldUpdateAuthor_WhenAuthorExists()
    {
        // Arrange
        var author = new Author("Old author");

        var request = new UpdateAuthorRequest
        {
            Name = "New author"
        };

        _authorRepositoryMock
            .Setup(x => x.GetByIdAsync(author.Id))
            .ReturnsAsync(author);

        var authorService = CreateService();

        // Act
        var result = await authorService.UpdateAsync(author.Id, request);

        // Assert
        Assert.Equal("New author", result.Name);

        _authorRepositoryMock.Verify(x => x.UpdateAsync(author), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowBadRequestException_WhenNameIsNotProvided()
    {
        // Arrange
        var authorService = CreateService();

        var request = new UpdateAuthorRequest();

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => authorService.UpdateAsync(Guid.NewGuid(), request));

        _authorRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);

        _authorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Author>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
    {
        // Arrange
        var authorId = Guid.NewGuid();

        var request = new UpdateAuthorRequest
        {
            Name = "New author"
        };

        _authorRepositoryMock
            .Setup(x => x.GetByIdAsync(authorId))
            .ReturnsAsync((Author)null!);

        var authorService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => authorService.UpdateAsync(authorId, request));
    }
}
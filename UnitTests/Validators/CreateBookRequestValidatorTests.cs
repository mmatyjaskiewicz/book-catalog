using Application.DTOs.Requests;
using Application.Validators;
using FluentValidation.TestHelper;

namespace UnitTests.Validators;

public class CreateBookRequestValidatorTests
{
    [Fact]
    public void ShouldBeValid_WhenRequestIsCorrect()
    {
        // Arrange
        var validator = new CreateBookRequestValidator();

        var request = new CreateBookRequest
        {
            Title = "Mock title",
            AuthorId = Guid.NewGuid(),
            PublishYear = 2024
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldHaveError_WhenTitleIsEmpty()
    {
        // Arrange
        var validator = new CreateBookRequestValidator();

        var request = new CreateBookRequest
        {
            Title = "",
            AuthorId = Guid.NewGuid(),
            PublishYear = 2024
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void ShouldHaveError_WhenTitleIsTooLong()
    {
        // Arrange
        var validator = new CreateBookRequestValidator();

        var request = new CreateBookRequest
        {
            Title = new string('A', 101),
            AuthorId = Guid.NewGuid(),
            PublishYear = 2024
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void ShouldHaveError_WhenAuthorIdIsEmpty()
    {
        // Arrange
        var validator = new CreateBookRequestValidator();

        var request = new CreateBookRequest
        {
            Title = "Mock title",
            AuthorId = Guid.Empty,
            PublishYear = 2024
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AuthorId);
    }

    [Fact]
    public void ShouldHaveError_WhenPublishYearIsBelowMinimum()
    {
        // Arrange
        var validator = new CreateBookRequestValidator();

        var request = new CreateBookRequest
        {
            Title = "Mock title",
            AuthorId = Guid.NewGuid(),
            PublishYear = 0
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishYear);
    }

    [Fact]
    public void ShouldHaveError_WhenPublishYearIsAboveCurrentYear()
    {
        // Arrange
        var validator = new CreateBookRequestValidator();

        var request = new CreateBookRequest
        {
            Title = "Mock title",
            AuthorId = Guid.NewGuid(),
            PublishYear = DateTime.Now.Year + 1
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishYear);
    }
}
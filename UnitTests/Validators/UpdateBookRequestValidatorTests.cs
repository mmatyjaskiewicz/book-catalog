using Application.DTOs.Requests;
using Application.Validators;
using FluentValidation.TestHelper;

namespace UnitTests.Validators;

public class UpdateBookRequestValidatorTests
{
    [Fact]
    public void ShouldBeValid_WhenRequestIsCorrect()
    {
        // Arrange
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
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
    public void ShouldHaveError_WhenTitleIsTooLong()
    {
        // Arrange
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
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
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
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
    public void ShouldHaveError_WhenYearIsBelowMinimum()
    {
        // Arrange
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
            Title = "Mock title",
            AuthorId = Guid.NewGuid(),
            PublishYear = -1
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishYear);
    }

    [Fact]
    public void ShouldHaveError_WhenYearIsAboveCurrentYear()
    {
        // Arrange
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
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
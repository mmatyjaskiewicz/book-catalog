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
    public void ShouldBeValid_WhenOnlyTitleIsProvided()
    {
        // Arrange
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
            Title = "New title"
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldBeValid_WhenOnlyAuthorIdIsProvided()
    {
        // Arrange
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
            AuthorId = Guid.NewGuid()
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldBeValid_WhenOnlyPublishYearIsProvided()
    {
        // Arrange
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
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
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
            Title = ""
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
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
            Title = new string('A', 101)
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
            AuthorId = Guid.Empty
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
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
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
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
            PublishYear = DateTime.Now.Year + 1
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishYear);
    }
}
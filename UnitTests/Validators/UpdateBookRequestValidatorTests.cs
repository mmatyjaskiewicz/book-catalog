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
            Author = "Mock author",
            Year = 2024
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
            Author = "Mock author",
            Year = 2024
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
    
    [Fact]
    public void ShouldHaveError_WhenAuthorIsTooLong()
    {
        // Arrange
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
            Title = "Mock title",
            Author = new string('A', 101),
            Year = 2024
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Author);
    }
    
    [Fact]
    public void ShouldHaveError_WhenYearIsBelowMinimum()
    {
        // Arrange
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
            Title = "Mock title",
            Author = "Mock author",
            Year = -1
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Year);
    }

    [Fact]
    public void ShouldHaveError_WhenYearIsAboveCurrentYear()
    {
        // Arrange
        var validator = new UpdateBookRequestValidator();

        var request = new UpdateBookRequest
        {
            Title = "Mock title",
            Author = "Mock author",
            Year = DateTime.Now.Year + 1
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Year);
    }
}
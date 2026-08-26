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
            Author = "Mock author",
            Year = 2024
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
            Author = "Mock author",
            Year = 2024
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
}
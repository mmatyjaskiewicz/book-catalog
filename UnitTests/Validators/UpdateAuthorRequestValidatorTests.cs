using Application.DTOs.Requests;
using Application.Validators;
using FluentValidation.TestHelper;

namespace UnitTests.Validators;

public class UpdateAuthorRequestValidatorTests
{
    [Fact]
    public void ShouldBeValid_WhenRequestIsCorrect()
    {
        // Arrange
        var validator = new UpdateAuthorRequestValidator();

        var request = new UpdateAuthorRequest
        {
            Name = "Mock author"
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var validator = new UpdateAuthorRequestValidator();

        var request = new UpdateAuthorRequest
        {
            Name = ""
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var validator = new UpdateAuthorRequestValidator();

        var request = new UpdateAuthorRequest
        {
            Name = new string('A', 101)
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldBeValid_WhenNameIsNull()
    {
        // Arrange
        var validator = new UpdateAuthorRequestValidator();

        var request = new UpdateAuthorRequest
        {
            Name = null
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldBeValid_WhenNameHasMaximumLength()
    {
        // Arrange
        var validator = new UpdateAuthorRequestValidator();

        var request = new UpdateAuthorRequest
        {
            Name = new string('A', 100)
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
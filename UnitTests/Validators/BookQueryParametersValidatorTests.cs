using Application.DTOs.Queries;
using Application.Validators;
using FluentValidation.TestHelper;

namespace UnitTests.Validators;

public class BookQueryParametersValidatorTests
{
    [Fact]
    public void ShouldBeValid_WhenParametersAreCorrect()
    {
        // Arrange
        var validator = new BookQueryParametersValidator();

        var queryParameters = new BookQueryParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = validator.TestValidate(queryParameters);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldHaveError_WhenPageNumberIsBelowMinimum()
    {
        // Arrange
        var validator = new BookQueryParametersValidator();

        var queryParameters = new BookQueryParameters
        {
            PageNumber = 0,
            PageSize = 10
        };

        // Act
        var result = validator.TestValidate(queryParameters);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Fact]
    public void ShouldHaveError_WhenPageSizeIsBelowMinimum()
    {
        // Arrange
        var validator = new BookQueryParametersValidator();

        var queryParameters = new BookQueryParameters
        {
            PageNumber = 1,
            PageSize = 0
        };

        // Act
        var result = validator.TestValidate(queryParameters);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void ShouldHaveError_WhenPageSizeIsAboveMaximum()
    {
        // Arrange
        var validator = new BookQueryParametersValidator();

        var queryParameters = new BookQueryParameters
        {
            PageNumber = 1,
            PageSize = 101
        };

        // Act
        var result = validator.TestValidate(queryParameters);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void ShouldBeValid_WhenPageSizeIsAtBoundaryValues()
    {
        // Arrange
        var validator = new BookQueryParametersValidator();

        var queryParameters = new BookQueryParameters
        {
            PageNumber = 1,
            PageSize = 100
        };

        // Act
        var result = validator.TestValidate(queryParameters);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
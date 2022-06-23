namespace CleanMinimalApi.Application.Tests.Unit.Authors.ReadById;

using CleanMinimalApi.Application.Authors.ReadById;
using FluentValidation.TestHelper;
using Xunit;

public class ReadByIdQueryValidatorTests
{
    private static readonly ReadByIdQueryValidator Validator = new();

    [Fact]
    public void ValidatorShouldHaveValidationErrorForIdNull()
    {
        // Arrange
        var query = new ReadByIdQuery();

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Id);
    }

    [Fact]
    public void ValidatorShouldHaveValidationErrorForIdEmpty()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Id);
    }

    [Fact]
    public void ValidatorShouldNotHaveValidationErrorForId()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.NewGuid() };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(query => query.Id);
    }
}

namespace CleanMinimalApi.Application.Tests.Unit.Reviews.ReadById;

using CleanMinimalApi.Application.Reviews.ReadById;
using FluentValidation.TestHelper;
using Xunit;

public class ReadByIdQueryValidatorTests
{
    private static readonly ReadByIdQueryValidator Validator = new();

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_IdNull()
    {
        // Arrange
        var query = new ReadByIdQuery();

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Id);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_IdEmpty()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Id);
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Id()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.NewGuid() };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(query => query.Id);
    }
}

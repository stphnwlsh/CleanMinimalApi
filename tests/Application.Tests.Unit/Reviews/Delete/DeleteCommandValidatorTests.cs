namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Delete;

using CleanMinimalApi.Application.Reviews.Delete;
using FluentValidation.TestHelper;
using Xunit;

public class DeleteCommandValidatorTests
{
    private static readonly DeleteCommandValidator Validator = new();

    [Fact]
    public void ValidatorShouldHaveValidationErrorForIdNull()
    {
        // Arrange
        var command = new DeleteCommand();

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Id);
    }

    [Fact]
    public void ValidatorShouldHaveValidationErrorForIdEmpty()
    {
        // Arrange
        var command = new DeleteCommand { Id = Guid.Empty };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Id);
    }

    [Fact]
    public void ValidatorShouldNotHaveValidationErrorForId()
    {
        // Arrange
        var command = new DeleteCommand { Id = Guid.NewGuid() };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Id);
    }
}

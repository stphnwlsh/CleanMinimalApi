namespace CleanMinimalApi.Presentation.Tests.Unit.Validators;

using CleanMinimalApi.Presentation.Validators;
using FluentValidation.TestHelper;
using Xunit;

public class GenericIdentityValidatorTests
{
    private static readonly GenericIdentityValidator Validator = new();

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Guid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var result = Validator.TestValidate(guid);

        // Assert
        result.ShouldNotHaveValidationErrorFor(guid => guid);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Empty()
    {
        // Arrange
        var guid = Guid.Empty;

        // Act
        var result = Validator.TestValidate(guid);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(guid => guid);
    }
}

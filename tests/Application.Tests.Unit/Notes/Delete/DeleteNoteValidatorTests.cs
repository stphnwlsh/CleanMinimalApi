namespace CleanMinimalApi.Application.Tests.Unit.Notes.Delete;

using System.Diagnostics.CodeAnalysis;
using CleanMinimalApi.Application.Notes.Delete;
using FluentValidation.TestHelper;
using Xunit;

[ExcludeFromCodeCoverage]
public class DeleteNoteValidatorTests
{
    private static readonly DeleteNoteValidator Validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(2147483647)]
    public void Validator_ShouldNotHaveValidationErrorFor_Id(int id)
    {
        // Arrange
        var command = new DeleteNoteCommand(id);

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Id);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-42)]
    [InlineData(-2147483647)]
    public void Validator_ShouldHaveValidationErrorFor_Id(int id)
    {
        // Arrange
        var command = new DeleteNoteCommand(id);

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Id);
    }
}

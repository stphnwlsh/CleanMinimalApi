namespace CleanMinimalApi.Application.Tests.Unit.Notes.Lookup;

using System.Diagnostics.CodeAnalysis;
using CleanMinimalApi.Application.Notes.Lookup;
using FluentValidation.TestHelper;
using Xunit;

[ExcludeFromCodeCoverage]
public class LookupNoteValidatorTests
{
    private static readonly LookupNoteValidator Validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(2147483647)]
    public void Validator_ShouldNotHaveValidationErrorFor_Id(int id)
    {
        // Arrange
        var command = new LookupNoteQuery(id);

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
        var command = new LookupNoteQuery(id);

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Id);
    }
}

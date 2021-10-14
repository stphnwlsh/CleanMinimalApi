namespace CleanMinimalApi.Application.Tests.Unit.Notes.Create;

using System.Diagnostics.CodeAnalysis;
using CleanMinimalApi.Application.Notes.Create;
using FluentValidation.TestHelper;
using Xunit;

[ExcludeFromCodeCoverage]
public class CreateNoteValidatorTests
{
    private static readonly CreateNoteValidator Validator = new();

    [Theory]
    [InlineData("NotInvalid")]
    [InlineData("Meaning of Life")]
    [InlineData("Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters ")]
    public void Validator_ShouldNotHaveValidationErrorFor_Text(string text)
    {
        // Arrange
        var command = new CreateNoteCommand(text);

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Text);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("                        ")]
    [InlineData("Invalid")]
    [InlineData("Three Hundred And One Characters Three Hundred And One Characters Three Hundred And One Characters Three Hundred And One Characters Three Hundred And One Characters Three Hundred And One Characters Three Hundred And One Characters Three Hundred And One Characters Three Hundred And One Characters Thre")]
    public void Validator_ShouldHaveValidationErrorFor_Text(string text)
    {
        // Arrange
        var command = new CreateNoteCommand(text);

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Text);
    }
}

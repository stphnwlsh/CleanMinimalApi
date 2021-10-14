namespace CleanMinimalApi.Application.Tests.Unit.Notes.Update;

using System.Diagnostics.CodeAnalysis;
using CleanMinimalApi.Application.Notes.Update;
using FluentValidation.TestHelper;
using Xunit;

[ExcludeFromCodeCoverage]
public class UpdateNoteValidatorTests
{
    private static readonly UpdateNoteValidator Validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(2147483647)]
    public void Validator_ShouldNotHaveValidationErrorFor_Id(int id)
    {
        // Arrange
        var command = new UpdateNoteCommand(id, "Meaning of Life");

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Id);
        result.ShouldNotHaveValidationErrorFor(command => command.Text);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-42)]
    [InlineData(-2147483647)]
    public void Validator_ShouldHaveValidationErrorFor_Id(int id)
    {
        // Arrange
        var command = new UpdateNoteCommand(id, "Meaning of Life");

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Id);
        result.ShouldNotHaveValidationErrorFor(command => command.Text);
    }

    [Theory]
    [InlineData("NotInvalid")]
    [InlineData("Meaning of Life")]
    [InlineData("Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters Three Hundred Characters ")]
    public void Validator_ShouldNotHaveValidationErrorFor_Text(string text)
    {
        // Arrange
        var command = new UpdateNoteCommand(42, text);

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Id);
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
        var command = new UpdateNoteCommand(42, text);

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Id);
        _ = result.ShouldHaveValidationErrorFor(command => command.Text);
    }
}

namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Commands.UpdateReview;

using Application.Reviews.Commands.UpdateReview;
using FluentValidation.TestHelper;
using Xunit;

public class UpdateReviewValidatorTests
{
    private static readonly UpdateReviewValidator Validator = new();

    [Fact]
    public void Validator_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Id()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.Empty,
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Validator_ShouldHaveValidationErrorFor_Stars(int stars)
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            Stars = stars
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Stars);
    }
}

namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Commands.DeleteReview;

using Application.Reviews.Commands.DeleteReview;
using FluentValidation.TestHelper;
using Xunit;

public class DeleteReviewValidatorTests
{
    private static readonly DeleteReviewValidator Validator = new();

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_IdNull()
    {
        // Arrange
        var command = new DeleteReviewCommand();

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Id);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_IdEmpty()
    {
        // Arrange
        var command = new DeleteReviewCommand { Id = Guid.Empty };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Id);
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Id()
    {
        // Arrange
        var command = new DeleteReviewCommand { Id = Guid.NewGuid() };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Id);
    }
}

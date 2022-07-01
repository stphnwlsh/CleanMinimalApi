namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Commands.UpdateReview;

using Application.Reviews.Commands.UpdateReview;
using FluentValidation.TestHelper;
using Xunit;

public class UpdateReviewValidatorTests
{
    private static readonly UpdateReviewValidator Validator = new();

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Id()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.NewGuid(),
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Id);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Id()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.Empty,
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.NewGuid(),
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Id).WithErrorMessage("A review id was not supplied to Update the review.");
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_ReviewAuthorId()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.NewGuid(),
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.AuthorId);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_ReviewAuthorId()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            AuthorId = Guid.Empty,
            MovieId = Guid.NewGuid(),
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.AuthorId).WithErrorMessage("An author id was not supplied to Update the review.");
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_ReviewedMovieId()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.NewGuid(),
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.MovieId);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_ReviewedMovieId()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.Empty,
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.MovieId).WithErrorMessage("A movie id was not supplied to Update the review.");
    }


    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validator_ShouldNotHaveValidationErrorFor_Stars(int stars)
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.NewGuid(),
            Stars = stars
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Stars);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(100)]
    [InlineData(-100)]
    public void Validator_ShouldHaveValidationErrorFor_Stars(int stars)
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.NewGuid(),
            Stars = stars
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Stars).WithErrorMessage("A star rating must be between 1 and 5.");
    }
}

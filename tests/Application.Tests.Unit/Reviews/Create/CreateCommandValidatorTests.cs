namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Create;

using CleanMinimalApi.Application.Reviews.Create;
using FluentValidation.TestHelper;
using Xunit;

public class CreateCommandValidatorTests
{
    private static readonly CreateCommandValidator Validator = new();

    [Fact]
    public void ValidatorShouldNotHaveValidationErrorForReviewAuthorId()
    {
        // Arrange
        var command = new CreateCommand
        {
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
    public void ValidatorShouldHaveValidationErrorForReviewAuthorId()
    {
        // Arrange
        var command = new CreateCommand
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.NewGuid(),
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.AuthorId).WithErrorMessage("An author id was not supplied to create the review.");
    }

    [Fact]
    public void ValidatorShouldNotHaveValidationErrorForReviewedMovieId()
    {
        // Arrange
        var command = new CreateCommand
        {
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
    public void ValidatorShouldHaveValidationErrorForReviewedMovieId()
    {
        // Arrange
        var command = new CreateCommand
        {
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.Empty,
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.MovieId).WithErrorMessage("A movie id was not supplied to create the review.");
    }


    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void ValidatorShouldNotHaveValidationErrorForStars(int stars)
    {
        // Arrange
        var command = new CreateCommand
        {
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
    public void ValidatorShouldHaveValidationErrorForStars(int stars)
    {
        // Arrange
        var command = new CreateCommand
        {
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

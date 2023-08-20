namespace CleanMinimalApi.Presentation.Tests.Unit.Validators;

using FluentValidation.TestHelper;
using Presentation.Requests;
using Presentation.Validators;
using Xunit;

public class CreateReviewValidatorTests
{
    private static readonly CreateReviewValidator Validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validator_ShouldNotHaveValidationErrorFor_ValidRequest(int stars)
    {
        // Arrange
        var command = new CreateReviewRequest
        {
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.NewGuid(),
            Stars = stars
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(request => request.AuthorId);
        result.ShouldNotHaveValidationErrorFor(request => request.MovieId);
        result.ShouldNotHaveValidationErrorFor(request => request.Stars);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_ReviewAuthorId()
    {
        // Arrange
        var command = new CreateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.NewGuid(),
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.AuthorId)
            .WithErrorMessage("An author id was not supplied to create the review.");

        result.ShouldNotHaveValidationErrorFor(request => request.MovieId);
        result.ShouldNotHaveValidationErrorFor(request => request.Stars);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_ReviewedMovieId()
    {
        // Arrange
        var command = new CreateReviewRequest
        {
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.Empty,
            Stars = 5
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.MovieId)
            .WithErrorMessage("A movie id was not supplied to create the review.");

        result.ShouldNotHaveValidationErrorFor(request => request.AuthorId);
        result.ShouldNotHaveValidationErrorFor(request => request.Stars);
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
        var command = new CreateReviewRequest
        {
            AuthorId = Guid.NewGuid(),
            MovieId = Guid.NewGuid(),
            Stars = stars
        };

        // Act
        var result = Validator.TestValidate(command);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(command => command.Stars)
            .WithErrorMessage("A star rating must be between 1 and 5.");

        result.ShouldNotHaveValidationErrorFor(request => request.AuthorId);
        result.ShouldNotHaveValidationErrorFor(request => request.MovieId);
    }
}

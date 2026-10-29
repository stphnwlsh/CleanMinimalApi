namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.GetReviewsForMovies;

using Application.Common.Paging;
using Application.Reviews.Queries.GetReviewsForMovies;
using FluentValidation.TestHelper;
using Xunit;

public class GetReviewsForMoviesValidatorTests
{
    private static readonly GetReviewsForMoviesValidator Validator = new();

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_NoIds()
    {
        // Arrange
        var query = new GetReviewsForMoviesQuery { MovieIds = [] };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.MovieIds);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_TooManyIds()
    {
        // Arrange
        var query = new GetReviewsForMoviesQuery { MovieIds = [.. Enumerable.Range(0, Paging.MaxLimit + 1).Select(_ => Guid.NewGuid())] };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.MovieIds);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_EmptyId()
    {
        // Arrange
        var query = new GetReviewsForMoviesQuery { MovieIds = [Guid.NewGuid(), Guid.Empty] };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor("MovieIds[1]");
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetReviewsForMoviesQuery { MovieIds = [Guid.NewGuid()] };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

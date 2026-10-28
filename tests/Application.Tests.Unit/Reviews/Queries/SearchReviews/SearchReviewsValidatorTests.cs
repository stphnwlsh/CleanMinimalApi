namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.SearchReviews;

using Application.Reviews.Queries.SearchReviews;
using FluentValidation.TestHelper;
using Xunit;

public class SearchReviewsValidatorTests
{
    private static readonly SearchReviewsValidator Validator = new();

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_MovieIdEmpty()
    {
        // Arrange
        var query = new SearchReviewsQuery { MovieId = Guid.Empty };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.MovieId);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_AuthorIdEmpty()
    {
        // Arrange
        var query = new SearchReviewsQuery { AuthorId = Guid.Empty };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.AuthorId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Validator_ShouldHaveValidationErrorFor_Limit(int limit)
    {
        // Arrange
        var query = new SearchReviewsQuery { Limit = limit };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Limit);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Cursor()
    {
        // Arrange
        var query = new SearchReviewsQuery { Cursor = "not a cursor" };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Cursor);
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new SearchReviewsQuery { MovieId = Guid.NewGuid(), AuthorId = Guid.NewGuid(), Limit = 50 };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrors_WithoutFilters()
    {
        // Arrange
        var query = new SearchReviewsQuery();

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

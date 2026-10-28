namespace CleanMinimalApi.Application.Tests.Unit.Movies.Queries.SearchMovies;

using Application.Movies.Queries.SearchMovies;
using FluentValidation.TestHelper;
using Xunit;

public class SearchMoviesValidatorTests
{
    private static readonly SearchMoviesValidator Validator = new();

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Validator_ShouldHaveValidationErrorFor_Limit(int limit)
    {
        // Arrange
        var query = new SearchMoviesQuery { Limit = limit };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Limit);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Cursor()
    {
        // Arrange
        var query = new SearchMoviesQuery { Cursor = "not a cursor" };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Cursor);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Query()
    {
        // Arrange
        var query = new SearchMoviesQuery { Query = new string('a', 101) };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Query);
    }

    [Theory]
    [InlineData(null, 1, null)]
    [InlineData("Lorem", 50, "b2Zmc2V0OjEw")]
    public void Validator_ShouldNotHaveValidationErrors(string search, int limit, string cursor)
    {
        // Arrange
        var query = new SearchMoviesQuery { Query = search, Limit = limit, Cursor = cursor };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

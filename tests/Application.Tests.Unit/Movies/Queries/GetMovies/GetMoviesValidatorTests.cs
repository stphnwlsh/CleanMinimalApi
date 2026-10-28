namespace CleanMinimalApi.Application.Tests.Unit.Movies.Queries.GetMovies;

using Application.Movies.Queries.GetMovies;
using FluentValidation.TestHelper;
using Xunit;

public class GetMoviesValidatorTests
{
    private static readonly GetMoviesValidator Validator = new();

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Validator_ShouldHaveValidationErrorFor_Limit(int limit)
    {
        // Arrange
        var query = new GetMoviesQuery { Limit = limit };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Limit);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Offset()
    {
        // Arrange
        var query = new GetMoviesQuery { Offset = -1 };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Offset);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Query()
    {
        // Arrange
        var query = new GetMoviesQuery { Query = new string('a', 101) };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Query);
    }

    [Theory]
    [InlineData(null, 0, 1)]
    [InlineData("Lorem", 100, 50)]
    public void Validator_ShouldNotHaveValidationErrors(string? search, int offset, int limit)
    {
        // Arrange
        var query = new GetMoviesQuery { Query = search, Offset = offset, Limit = limit };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

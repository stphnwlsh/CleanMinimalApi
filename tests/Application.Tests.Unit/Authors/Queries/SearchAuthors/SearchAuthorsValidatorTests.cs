namespace CleanMinimalApi.Application.Tests.Unit.Authors.Queries.SearchAuthors;

using Application.Authors.Queries.SearchAuthors;
using FluentValidation.TestHelper;
using Xunit;

public class SearchAuthorsValidatorTests
{
    private static readonly SearchAuthorsValidator Validator = new();

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Validator_ShouldHaveValidationErrorFor_Limit(int limit)
    {
        // Arrange
        var query = new SearchAuthorsQuery { Limit = limit };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Limit);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Cursor()
    {
        // Arrange
        var query = new SearchAuthorsQuery { Cursor = "not a cursor" };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Cursor);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Query()
    {
        // Arrange
        var query = new SearchAuthorsQuery { Query = new string('a', 101) };

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
        var query = new SearchAuthorsQuery { Query = search, Limit = limit, Cursor = cursor };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

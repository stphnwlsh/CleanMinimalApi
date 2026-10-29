namespace CleanMinimalApi.Application.Tests.Unit.Authors.Queries.GetAuthors;

using Application.Authors.Queries.GetAuthors;
using FluentValidation.TestHelper;
using Xunit;

public class GetAuthorsValidatorTests
{
    private static readonly GetAuthorsValidator Validator = new();

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Validator_ShouldHaveValidationErrorFor_Limit(int limit)
    {
        // Arrange
        var query = new GetAuthorsQuery { Limit = limit };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Limit);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Offset()
    {
        // Arrange
        var query = new GetAuthorsQuery { Offset = -1 };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Offset);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Query()
    {
        // Arrange
        var query = new GetAuthorsQuery { Query = new string('a', 101) };

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
        var query = new GetAuthorsQuery { Query = search, Offset = offset, Limit = limit };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

namespace CleanMinimalApi.Application.Tests.Unit.Movies.Queries.GetMovieById;

using Application.Movies.Queries.GetMovieById;
using FluentValidation.TestHelper;
using Xunit;

public class GetMovieByIdValidatorTests
{
    private static readonly GetMovieByIdValidator Validator = new();

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_IdNull()
    {
        // Arrange
        var query = new GetMovieByIdQuery();

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Id);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_IdEmpty()
    {
        // Arrange
        var query = new GetMovieByIdQuery { Id = Guid.Empty };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Id);
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Id()
    {
        // Arrange
        var query = new GetMovieByIdQuery { Id = Guid.NewGuid() };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(query => query.Id);
    }
}

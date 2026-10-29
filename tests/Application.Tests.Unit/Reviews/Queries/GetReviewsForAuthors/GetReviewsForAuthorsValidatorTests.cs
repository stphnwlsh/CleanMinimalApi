namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.GetReviewsForAuthors;

using Application.Common.Paging;
using Application.Reviews.Queries.GetReviewsForAuthors;
using FluentValidation.TestHelper;
using Xunit;

public class GetReviewsForAuthorsValidatorTests
{
    private static readonly GetReviewsForAuthorsValidator Validator = new();

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_NoIds()
    {
        // Arrange
        var query = new GetReviewsForAuthorsQuery { AuthorIds = [] };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.AuthorIds);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_TooManyIds()
    {
        // Arrange
        var query = new GetReviewsForAuthorsQuery { AuthorIds = [.. Enumerable.Range(0, Paging.MaxLimit + 1).Select(_ => Guid.NewGuid())] };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.AuthorIds);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_EmptyId()
    {
        // Arrange
        var query = new GetReviewsForAuthorsQuery { AuthorIds = [Guid.NewGuid(), Guid.Empty] };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor("AuthorIds[1]");
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetReviewsForAuthorsQuery { AuthorIds = [Guid.NewGuid()] };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

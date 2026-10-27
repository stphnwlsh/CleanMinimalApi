namespace CleanMinimalApi.Application.Tests.Unit.Authors.Queries.GetAuthorById;

using Application.Authors.Queries.GetAuthorById;
using FluentValidation.TestHelper;
using Xunit;

public class GetAuthorByIdQueryValidatorTests
{
    private static readonly GetAuthorByIdValidator Validator = new();

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_IdNull()
    {
        // Arrange
        var query = new GetAuthorByIdQuery();

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Id);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_IdEmpty()
    {
        // Arrange
        var query = new GetAuthorByIdQuery { Id = Guid.Empty };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        _ = result.ShouldHaveValidationErrorFor(query => query.Id);
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Id()
    {
        // Arrange
        var query = new GetAuthorByIdQuery { Id = Guid.NewGuid() };

        // Act
        var result = Validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(query => query.Id);
    }
}

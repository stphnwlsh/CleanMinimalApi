namespace CleanMinimalApi.Application.Tests.Unit.Common.Exceptions;

using Application.Authors.Entities;
using CleanMinimalApi.Application.Common.Enums;
using CleanMinimalApi.Application.Common.Exceptions;
using Shouldly;
using Xunit;

public class NotFoundExceptionTests
{
    [Fact]
    public void ThrowIfNull_ShouldNotThrow_NotFoundException()
    {
        // Arrange
        var entityType = EntityType.Author;
        var argument = new Author(Guid.NewGuid(), "FirstName", "LastName");

        // Act
        var result = Should.NotThrow(() =>
        {
            NotFoundException.ThrowIfNull(argument, entityType);

            return true;
        });

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void ThrowIfNull_ShouldThrow_NotFoundException()
    {
        // Arrange
        var entityType = EntityType.Author;
        Author argument = null;

        // Act
        var result = Should.Throw<NotFoundException>(() =>
        {
            NotFoundException.ThrowIfNull(argument, entityType);

            return true;
        });

        // Assert
        _ = result.ShouldNotBeNull();

        result.Message.ShouldBe("The Author with the supplied id was not found.");
    }

    [Fact]
    public void Throw_ShouldThrow_NotFoundException()
    {
        // Arrange
        var entityType = EntityType.Author;

        // Act
        var result = Should.Throw<NotFoundException>(() =>
        {
            NotFoundException.Throw(entityType);

            return true;
        });

        // Assert
        _ = result.ShouldNotBeNull();

        result.Message.ShouldBe("The Author with the supplied id was not found.");
    }
}

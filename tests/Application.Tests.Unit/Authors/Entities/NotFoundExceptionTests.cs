namespace CleanMinimalApi.Application.Tests.Unit.Common.Exceptions;

using Application.Authors.Entities;
using Shouldly;
using Xunit;

public class EntityTests
{
    [Fact]
    public void Author_Should_Have_Default_Values()
    {
        // Arrange & Act
        var author = new Author();

        // Assert
        author.Id.ShouldBe(Guid.Empty);
        author.FirstName.ShouldBeNull();
        author.LastName.ShouldBeNull();
        author.DateCreated.ShouldBe(DateTime.MinValue);
        author.DateModified.ShouldBe(DateTime.MinValue);
    }
}

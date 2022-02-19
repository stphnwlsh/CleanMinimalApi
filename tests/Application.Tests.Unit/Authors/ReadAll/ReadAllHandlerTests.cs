namespace CleanMinimalApi.Application.Tests.Unit.Authors.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors.ReadAll;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

public class ReadAllHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadAllQuery();

        var context = Substitute.For<AuthorsRepository>();
        var handler = new ReadAllHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.ReadAllAuthors(token).Returns(new List<Author> {
            new Author
            {
                Id = Guid.Empty,
                FirstName = "FirstName",
                LastName = "LastName"
            }
        });

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadAllAuthors(token);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(Guid.Empty);
        result[0].FirstName.ShouldBe("FirstName");
        result[0].LastName.ShouldBe("LastName");
    }
}

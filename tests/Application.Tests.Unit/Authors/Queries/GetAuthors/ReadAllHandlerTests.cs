namespace CleanMinimalApi.Application.Tests.Unit.Authors.Queries.GetAuthors;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors;
using CleanMinimalApi.Application.Authors.Entities;
using CleanMinimalApi.Application.Authors.Queries.GetAuthors;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetAuthorsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetAuthorsQuery();

        var context = Substitute.For<IAuthorsRepository>();
        var handler = new GetAuthorsHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.GetAuthors(token).Returns(new List<Author> {
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
        _ = await context.Received(1).GetAuthors(token);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(Guid.Empty);
        result[0].FirstName.ShouldBe("FirstName");
        result[0].LastName.ShouldBe("LastName");
    }
}

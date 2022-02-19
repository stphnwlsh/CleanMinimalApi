namespace CleanMinimalApi.Application.Tests.Unit.Movies.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Entities;
using CleanMinimalApi.Application.Movies.ReadAll;
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

        var context = Substitute.For<MoviesRepository>();
        var handler = new ReadAllHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.ReadAllMovies(token).Returns(new List<Movie> {
            new Movie{
                Id = Guid.Empty,
                Title = "Title"
            }
        });

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadAllMovies(token);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(Guid.Empty);
        result[0].Title.ShouldBe("Title");
    }
}

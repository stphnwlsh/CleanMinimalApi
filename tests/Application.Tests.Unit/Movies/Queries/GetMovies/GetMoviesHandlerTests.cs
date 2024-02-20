namespace CleanMinimalApi.Application.Tests.Unit.Movies.Queries.GetMovies;

using System.Threading;
using System.Threading.Tasks;
using Application.Movies;
using Application.Movies.Entities;
using Application.Movies.Queries.GetMovies;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetMoviesHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetMoviesQuery();

        var context = Substitute.For<IMoviesRepository>();
        var handler = new GetMoviesHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.GetMovies(token).Returns([new Movie(Guid.Empty, "Title")]);

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).GetMovies(token);

        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<List<Movie>>();

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);

        result[0].Id.ShouldBe(Guid.Empty);
        result[0].Title.ShouldBe("Title");
    }
}

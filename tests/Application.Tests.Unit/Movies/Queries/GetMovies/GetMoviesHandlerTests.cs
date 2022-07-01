namespace CleanMinimalApi.Application.Tests.Unit.Movies.Queries.GetMovies;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Movies;
using CleanMinimalApi.Application.Movies.Entities;
using CleanMinimalApi.Application.Movies.Queries.GetMovies;
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

        _ = context.GetMovies(token).Returns(new List<Movie> {
            new Movie{
                Id = Guid.Empty,
                Title = "Title"
            }
        });

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).GetMovies(token);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(Guid.Empty);
        result[0].Title.ShouldBe("Title");
    }
}

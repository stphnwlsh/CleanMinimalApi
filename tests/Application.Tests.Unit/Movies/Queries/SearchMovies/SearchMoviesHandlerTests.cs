namespace CleanMinimalApi.Application.Tests.Unit.Movies.Queries.SearchMovies;

using System.Threading;
using System.Threading.Tasks;
using Application.Movies;
using Application.Movies.Entities;
using Application.Movies.Queries.SearchMovies;
using Application.Common.Paging;
using NSubstitute;
using Shouldly;
using Xunit;

public class SearchMoviesHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new SearchMoviesQuery { Query = "Lorem", Limit = 1, Cursor = Paging.Encode(2) };

        var repository = Substitute.For<IMoviesRepository>();
        var handler = new SearchMoviesHandler(repository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = repository.SearchMovies("Lorem", 2, 1, token).Returns(([new Movie(Guid.Empty, "Title")], 5));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await repository.Received(1).SearchMovies("Lorem", 2, 1, token);

        _ = result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(1);
        result.Items[0].Title.ShouldBe("Title");
        result.TotalCount.ShouldBe(5);
        result.NextCursor.ShouldBe(Paging.Encode(3));
    }

    [Fact]
    public async Task Handle_ShouldStartAt_FirstPage()
    {
        // Arrange
        var query = new SearchMoviesQuery();

        var repository = Substitute.For<IMoviesRepository>();
        var handler = new SearchMoviesHandler(repository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = repository.SearchMovies(null, 0, Paging.DefaultLimit, token).Returns(([new Movie(Guid.Empty, "Title")], 1));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await repository.Received(1).SearchMovies(null, 0, Paging.DefaultLimit, token);

        result.TotalCount.ShouldBe(1);
        result.NextCursor.ShouldBeNull();
    }
}

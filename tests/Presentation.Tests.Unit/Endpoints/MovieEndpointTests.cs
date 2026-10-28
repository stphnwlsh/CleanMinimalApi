namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors.Entities;
using CleanMinimalApi.Application.Common.Paging;
using CleanMinimalApi.Application.Movies.Entities;
using CleanMinimalApi.Presentation.Endpoints;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Shouldly;
using Xunit;
using Entities = Application.Movies.Entities;
using Queries = Application.Movies.Queries;

public class MovieEndpointTests
{
    [Fact]
    public async Task GetMovies_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = sender
            .Send(Arg.Is<Queries.GetMovies.GetMoviesQuery>(q => q.Query == "Lorem" && q.Offset == 20 && q.Limit == 5), token)
            .Returns(new Page<Entities.Movie>([new Entities.Movie(Guid.Empty, "Lorem Ipsum")], 20, 5, 21));

        // Act
        var response = await MovieEndpoints.GetMovies("Lorem", sender, token, 20, 5);

        // Assert
        var result = response.ShouldBeOfType<Ok<Page<Entities.Movie>>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldNotBeNull();

        value.Items.Count.ShouldBe(1);
        value.Items[0].Id.ShouldBe(Guid.Empty);
        value.Items[0].Title.ShouldBe("Lorem Ipsum");
        value.Items[0].Reviews.ShouldBeNull();
        value.Offset.ShouldBe(20);
        value.Limit.ShouldBe(5);
        value.TotalCount.ShouldBe(21);
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var id = Guid.NewGuid();
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = sender
            .Send(Arg.Is<Queries.GetMovieById.GetMovieByIdQuery>(q => q.Id == id), token)
            .Returns(new Entities.Movie(id, "Lorem Ipsum"));

        // Act
        var response = await MovieEndpoints.GetMovieById(id, sender, token);

        // Assert
        var result = response.ShouldBeOfType<Ok<Entities.Movie>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldNotBeNull();

        value.Id.ShouldBe(id);
        value.Title.ShouldBe("Lorem Ipsum");
        value.Reviews.ShouldBeNull();
    }
}

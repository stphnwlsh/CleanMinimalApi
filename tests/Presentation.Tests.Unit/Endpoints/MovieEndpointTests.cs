namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Reviews.Entities;
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

        _ = sender
            .Send(Arg.Any<Queries.GetMovies.GetMoviesQuery>())
            .ReturnsForAnyArgs(
            [
                new Entities.Movie(Guid.Empty, "Lorem Ipsum")
            ]);

        // Act
        var response = await MovieEndpoints.GetMovies(sender);

        // Assert
        var result = response.ShouldBeOfType<Ok<List<Entities.Movie>>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<List<Entities.Movie>>();

        _ = value[0].Id.ShouldBeOfType<Guid>();
        value[0].Id.ShouldBe(Guid.Empty);
        _ = value[0].Title.ShouldBeOfType<string>();
        value[0].Title.ShouldBe("Lorem Ipsum");
        _ = value[0].Reviews.ShouldBeAssignableTo<ICollection<Review>>();
        value[0].Reviews.ShouldBeNull();
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetMovieById.GetMovieByIdQuery>())
            .ReturnsForAnyArgs(new Entities.Movie(Guid.Empty, "Lorem Ipsum"));

        // Act
        var response = await MovieEndpoints.GetMovieById(Guid.Empty, sender);

        // Assert
        var result = response.ShouldBeOfType<Ok<Entities.Movie>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<Entities.Movie>();

        _ = value.Id.ShouldBeOfType<Guid>();
        value.Id.ShouldBe(Guid.Empty);
        _ = value.Title.ShouldBeOfType<string>();
        value.Title.ShouldBe("Lorem Ipsum");
        _ = value.Reviews.ShouldBeAssignableTo<ICollection<Review>>();
        value.Reviews.ShouldBeNull();
    }
}

namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Reviews.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Presentation.Endpoints;
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
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Queries.GetMovies.GetMoviesQuery>())
            .ReturnsForAnyArgs(
            [
                new Entities.Movie(Guid.Empty, "Lorem Ipsum")
            ]);

        // Act
        var response = await MoviesEndpoints.GetMovies(mediator);

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
    public async Task GetMovies_ShouldReturn_Problem()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Queries.GetMovies.GetMoviesQuery>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await MoviesEndpoints.GetMovies(mediator);

        // Assert
        var result = response.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_Ok()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Queries.GetMovieById.GetMovieByIdQuery>())
            .ReturnsForAnyArgs(new Entities.Movie(Guid.Empty, "Lorem Ipsum"));

        // Act
        var response = await MoviesEndpoints.GetMovieById(Guid.Empty, mediator);

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

    [Fact]
    public async Task GetMovieById_ShouldReturn_NotFound()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Queries.GetMovieById.GetMovieByIdQuery>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await MoviesEndpoints.GetMovieById(Guid.Empty, mediator);

        // Assert
        var result = response.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_Problem()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Queries.GetMovieById.GetMovieByIdQuery>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await MoviesEndpoints.GetMovieById(Guid.Empty, mediator);

        // Assert
        var result = response.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }
}

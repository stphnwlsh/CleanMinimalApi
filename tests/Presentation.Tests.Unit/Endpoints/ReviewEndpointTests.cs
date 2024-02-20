namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors.Entities;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Movies.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Presentation.Endpoints;
using Shouldly;
using Xunit;
using Commands = Application.Reviews.Commands;
using Entities = Application.Reviews.Entities;
using Queries = Application.Reviews.Queries;

public class ReviewEndpointTests
{
    [Fact]
    public async Task GetReviews_ShouldReturn_Ok()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Queries.GetReviews.GetReviewsQuery>())
            .ReturnsForAnyArgs(
            [
                new Entities.Review(
                    Guid.Empty,
                    5,
                    new ReviewedMovie(Guid.Empty, "Lorem Ipsum"),
                    new ReviewAuthor(Guid.Empty, "Lorem", "Ipsum")
                )
            ]);

        // Act
        var response = await ReviewsEndpoints.GetReviews(mediator);

        // Assert
        var result = response.ShouldBeOfType<Ok<List<Entities.Review>>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<List<Entities.Review>>();

        _ = value[0].Id.ShouldBeOfType<Guid>();
        value[0].Id.ShouldBe(Guid.Empty);
        _ = value[0].Stars.ShouldBeOfType<int>();
        value[0].Stars.ShouldBe(5);

        _ = value[0].ReviewAuthor.Id.ShouldBeOfType<Guid>();
        value[0].ReviewAuthor.Id.ShouldBe(Guid.Empty);
        _ = value[0].ReviewAuthor.FirstName.ShouldBeOfType<string>();
        value[0].ReviewAuthor.FirstName.ShouldBe("Lorem");
        _ = value[0].ReviewAuthor.LastName.ShouldBeOfType<string>();
        value[0].ReviewAuthor.LastName.ShouldBe("Ipsum");

        _ = value[0].ReviewedMovie.Id.ShouldBeOfType<Guid>();
        value[0].ReviewedMovie.Id.ShouldBe(Guid.Empty);
        _ = value[0].ReviewedMovie.Title.ShouldBeOfType<string>();
        value[0].ReviewedMovie.Title.ShouldBe("Lorem Ipsum");
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_Problem()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Queries.GetReviews.GetReviewsQuery>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await ReviewsEndpoints.GetReviews(mediator);

        // Assert
        var result = response.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetReviewById_ShouldReturn_Ok()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Queries.GetReviewById.GetReviewByIdQuery>())
            .ReturnsForAnyArgs(new Entities.Review(
                Guid.Empty,
                5,
                new ReviewedMovie(Guid.Empty, "Lorem Ipsum"),
                new ReviewAuthor(Guid.Empty, "Lorem", "Ipsum")
            ));

        // Act
        var response = await ReviewsEndpoints.GetReviewById(Guid.Empty, mediator);

        // Assert
        var result = response.ShouldBeOfType<Ok<Entities.Review>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<Entities.Review>();

        _ = value.Id.ShouldBeOfType<Guid>();
        value.Id.ShouldBe(Guid.Empty);
        _ = value.Stars.ShouldBeOfType<int>();
        value.Stars.ShouldBe(5);

        _ = value.ReviewAuthor.Id.ShouldBeOfType<Guid>();
        value.ReviewAuthor.Id.ShouldBe(Guid.Empty);
        _ = value.ReviewAuthor.FirstName.ShouldBeOfType<string>();
        value.ReviewAuthor.FirstName.ShouldBe("Lorem");
        _ = value.ReviewAuthor.LastName.ShouldBeOfType<string>();
        value.ReviewAuthor.LastName.ShouldBe("Ipsum");

        _ = value.ReviewedMovie.Id.ShouldBeOfType<Guid>();
        value.ReviewedMovie.Id.ShouldBe(Guid.Empty);
        _ = value.ReviewedMovie.Title.ShouldBeOfType<string>();
        value.ReviewedMovie.Title.ShouldBe("Lorem Ipsum");
    }

    [Fact]
    public async Task GetReviewById_ShouldReturn_NotFound()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Queries.GetReviewById.GetReviewByIdQuery>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await ReviewsEndpoints.GetReviewById(Guid.Empty, mediator);

        // Assert
        var result = response.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task GetReviewById_ShouldReturn_Problem()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Queries.GetReviewById.GetReviewByIdQuery>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await ReviewsEndpoints.GetReviewById(Guid.Empty, mediator);

        // Assert
        var result = response.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreateReview_ShouldReturn_Created()
    {
        // Arrange
        //var httpRequest = Substitute.For<HttpRequest>();
        var mediator = Substitute.For<IMediator>();
        var request = new Requests.CreateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = mediator
            .Send(Arg.Any<Commands.CreateReview.CreateReviewCommand>())
            .ReturnsForAnyArgs(new Entities.Review(
                Guid.Empty,
                5,
                new ReviewedMovie(Guid.Empty, "Lorem Ipsum"),
                new ReviewAuthor(Guid.Empty, "Lorem", "Ipsum")
            ));

        // Act
        var response = await ReviewsEndpoints.CreateReview(request, mediator);

        // Assert
        var result = response.ShouldBeOfType<Created<Entities.Review>>();

        result.StatusCode.ShouldBe(StatusCodes.Status201Created);

        var value = result.Value.ShouldBeOfType<Entities.Review>();

        _ = value.Id.ShouldBeOfType<Guid>();
        value.Id.ShouldBe(Guid.Empty);
        _ = value.Stars.ShouldBeOfType<int>();
        value.Stars.ShouldBe(5);

        _ = value.ReviewAuthor.Id.ShouldBeOfType<Guid>();
        value.ReviewAuthor.Id.ShouldBe(Guid.Empty);
        _ = value.ReviewAuthor.FirstName.ShouldBeOfType<string>();
        value.ReviewAuthor.FirstName.ShouldBe("Lorem");
        _ = value.ReviewAuthor.LastName.ShouldBeOfType<string>();
        value.ReviewAuthor.LastName.ShouldBe("Ipsum");

        _ = value.ReviewedMovie.Id.ShouldBeOfType<Guid>();
        value.ReviewedMovie.Id.ShouldBe(Guid.Empty);
        _ = value.ReviewedMovie.Title.ShouldBeOfType<string>();
        value.ReviewedMovie.Title.ShouldBe("Lorem Ipsum");
    }

    [Fact]
    public async Task CreateReview_ShouldReturn_NotFound()
    {
        // Arrange
        //var httpRequest = Substitute.For<HttpRequest>();
        var mediator = Substitute.For<IMediator>();
        var request = new Requests.CreateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = mediator
            .Send(Arg.Any<Commands.CreateReview.CreateReviewCommand>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await ReviewsEndpoints.CreateReview(request, mediator);

        // Assert
        var result = response.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task CreateReview_ShouldReturn_Problem()
    {
        // Arrange
        var httpRequest = Substitute.For<HttpRequest>();
        var mediator = Substitute.For<IMediator>();
        var request = new Requests.CreateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = mediator
            .Send(Arg.Any<Commands.CreateReview.CreateReviewCommand>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await ReviewsEndpoints.CreateReview(request, mediator);

        // Assert
        var result = response.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_NoContent()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var request = new Requests.UpdateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = mediator
            .Send(Arg.Any<Commands.UpdateReview.UpdateReviewCommand>())
            .ReturnsForAnyArgs(true);

        // Act
        var response = await ReviewsEndpoints.UpdateReview(Guid.NewGuid(), request, mediator);

        // Assert
        var result = response.ShouldBeOfType<NoContent>();

        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_NotFound()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var request = new Requests.UpdateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = mediator
            .Send(Arg.Any<Commands.UpdateReview.UpdateReviewCommand>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await ReviewsEndpoints.UpdateReview(Guid.NewGuid(), request, mediator);

        // Assert
        var result = response.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_Problem()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var request = new Requests.UpdateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = mediator
            .Send(Arg.Any<Commands.UpdateReview.UpdateReviewCommand>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await ReviewsEndpoints.UpdateReview(Guid.NewGuid(), request, mediator);

        // Assert
        var result = response.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_NoContent()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Commands.DeleteReview.DeleteReviewCommand>())
            .ReturnsForAnyArgs(true);

        // Act
        var response = await ReviewsEndpoints.DeleteReview(Guid.NewGuid(), mediator);

        // Assert
        var result = response.ShouldBeOfType<NoContent>();

        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_NotFound()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Commands.DeleteReview.DeleteReviewCommand>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await ReviewsEndpoints.DeleteReview(Guid.NewGuid(), mediator);

        // Assert
        var result = response.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_Problem()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator
            .Send(Arg.Any<Commands.DeleteReview.DeleteReviewCommand>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await ReviewsEndpoints.DeleteReview(Guid.Empty, mediator);

        // Assert
        var result = response.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }
}

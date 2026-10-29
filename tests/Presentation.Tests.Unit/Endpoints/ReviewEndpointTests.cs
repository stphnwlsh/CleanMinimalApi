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
using Commands = Application.Reviews.Commands;
using Entities = Application.Reviews.Entities;
using Queries = Application.Reviews.Queries;

public class ReviewEndpointTests
{
    private static readonly Entities.Review Review = new(
        Guid.NewGuid(),
        5,
        new ReviewedMovie(Guid.NewGuid(), "Lorem Ipsum"),
        new ReviewAuthor(Guid.NewGuid(), "Lorem", "Ipsum"));

    [Fact]
    public async Task GetReviews_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var movieId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = sender
            .Send(
                Arg.Is<Queries.GetReviews.GetReviewsQuery>(q =>
                    q.MovieId == movieId && q.AuthorId == authorId && q.Offset == 20 && q.Limit == 5),
                token)
            .Returns(new Page<Entities.Review>([Review], 20, 5, 21));

        // Act
        var response = await ReviewEndpoints.GetReviews(movieId, authorId, sender, token, 20, 5);

        // Assert
        var result = response.ShouldBeOfType<Ok<Page<Entities.Review>>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldNotBeNull();

        value.Items.ShouldBe([Review]);
        value.Offset.ShouldBe(20);
        value.Limit.ShouldBe(5);
        value.TotalCount.ShouldBe(21);
    }

    [Fact]
    public async Task GetReviewById_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = sender
            .Send(Arg.Is<Queries.GetReviewById.GetReviewByIdQuery>(q => q.Id == Review.Id), token)
            .Returns(Review);

        // Act
        var response = await ReviewEndpoints.GetReviewById(Review.Id, sender, token);

        // Assert
        var result = response.ShouldBeOfType<Ok<Entities.Review>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        result.Value.ShouldBe(Review);
    }

    [Fact]
    public async Task CreateReview_ShouldReturn_CreatedAtRoute()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var request = new Requests.CreateReviewRequest
        {
            AuthorId = Review.ReviewAuthor!.Id,
            MovieId = Review.ReviewedMovie!.Id,
            Stars = 5
        };
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = sender
            .Send(
                Arg.Is<Commands.CreateReview.CreateReviewCommand>(c =>
                    c.AuthorId == request.AuthorId && c.MovieId == request.MovieId && c.Stars == 5),
                token)
            .Returns(Review);

        // Act
        var response = await ReviewEndpoints.CreateReview(request, sender, token);

        // Assert
        var result = response.ShouldBeOfType<CreatedAtRoute<Entities.Review>>();

        result.StatusCode.ShouldBe(StatusCodes.Status201Created);
        result.RouteName.ShouldBe(nameof(ReviewEndpoints.GetReviewById));
        result.RouteValues["id"].ShouldBe(Review.Id);
        result.Value.ShouldBe(Review);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_NoContent()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var id = Guid.NewGuid();
        var request = new Requests.UpdateReviewRequest
        {
            Stars = 4
        };
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = sender
            .Send(Arg.Is<Commands.UpdateReview.UpdateReviewCommand>(c => c.Id == id && c.Stars == 4), token)
            .Returns(true);

        // Act
        var response = await ReviewEndpoints.UpdateReview(id, request, sender, token);

        // Assert
        var result = response.ShouldBeOfType<NoContent>();

        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);

        _ = await sender.Received(1).Send(Arg.Is<Commands.UpdateReview.UpdateReviewCommand>(c => c.Id == id && c.Stars == 4), token);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_NoContent()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var id = Guid.NewGuid();
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = sender
            .Send(Arg.Is<Commands.DeleteReview.DeleteReviewCommand>(c => c.Id == id), token)
            .Returns(true);

        // Act
        var response = await ReviewEndpoints.DeleteReview(id, sender, token);

        // Assert
        var result = response.ShouldBeOfType<NoContent>();

        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);

        _ = await sender.Received(1).Send(Arg.Is<Commands.DeleteReview.DeleteReviewCommand>(c => c.Id == id), token);
    }
}

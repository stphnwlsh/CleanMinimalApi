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
    [Fact]
    public async Task GetReviews_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
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
        var response = await ReviewEndpoints.GetReviews(sender);

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
    public async Task GetReviewById_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetReviewById.GetReviewByIdQuery>())
            .ReturnsForAnyArgs(new Entities.Review(
                Guid.Empty,
                5,
                new ReviewedMovie(Guid.Empty, "Lorem Ipsum"),
                new ReviewAuthor(Guid.Empty, "Lorem", "Ipsum")
            ));

        // Act
        var response = await ReviewEndpoints.GetReviewById(Guid.Empty, sender);

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
    public async Task CreateReview_ShouldReturn_Created()
    {
        // Arrange
        //var httpRequest = Substitute.For<HttpRequest>();
        var sender = Substitute.For<ISender>();
        var request = new Requests.CreateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = sender
            .Send(Arg.Any<Commands.CreateReview.CreateReviewCommand>())
            .ReturnsForAnyArgs(new Entities.Review(
                Guid.Empty,
                5,
                new ReviewedMovie(Guid.Empty, "Lorem Ipsum"),
                new ReviewAuthor(Guid.Empty, "Lorem", "Ipsum")
            ));

        // Act
        var response = await ReviewEndpoints.CreateReview(request, sender);

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
    public async Task UpdateReview_ShouldReturn_NoContent()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var request = new Requests.UpdateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = sender
            .Send(Arg.Any<Commands.UpdateReview.UpdateReviewCommand>())
            .ReturnsForAnyArgs(true);

        // Act
        var response = await ReviewEndpoints.UpdateReview(Guid.NewGuid(), request, sender);

        // Assert
        var result = response.ShouldBeOfType<NoContent>();

        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_NoContent()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Commands.DeleteReview.DeleteReviewCommand>())
            .ReturnsForAnyArgs(true);

        // Act
        var response = await ReviewEndpoints.DeleteReview(Guid.NewGuid(), sender);

        // Assert
        var result = response.ShouldBeOfType<NoContent>();

        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task SearchReviews_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var movieId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        _ = sender
            .Send(Arg.Is<Queries.SearchReviews.SearchReviewsQuery>(q =>
                q.MovieId == movieId && q.AuthorId == authorId && q.Limit == 5 && q.Cursor == "cursor"))
            .Returns(new Page<Entities.Review>(
            [
                new Entities.Review(
                    Guid.Empty,
                    5,
                    new ReviewedMovie(movieId, "Lorem Ipsum"),
                    new ReviewAuthor(authorId, "Lorem", "Ipsum")
                )
            ], 6, "next"));

        // Act
        var response = await ReviewEndpoints.SearchReviews(movieId, authorId, sender, 5, "cursor");

        // Assert
        var result = response.ShouldBeOfType<Ok<Page<Entities.Review>>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<Page<Entities.Review>>();

        value.TotalCount.ShouldBe(6);
        value.NextCursor.ShouldBe("next");
        _ = value.Items.ShouldHaveSingleItem();
        value.Items[0].Stars.ShouldBe(5);
        value.Items[0].ReviewedMovie.Id.ShouldBe(movieId);
        value.Items[0].ReviewAuthor.Id.ShouldBe(authorId);
    }
}

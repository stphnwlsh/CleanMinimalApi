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
            .ReturnsForAnyArgs(new List<Entities.Review>
            {
                new Entities.Review
                {
                    Id = Guid.Empty,
                    Stars = 5,
                    ReviewAuthor = new Author
                    {
                        Id = Guid.Empty,
                        FirstName = "Lorem",
                        LastName = "Ipsum"
                    },
                    ReviewedMovie = new Movie
                    {
                        Id = Guid.Empty,
                        Title = "Lorem Ipsum"
                    }
                }
            });

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
            .ReturnsForAnyArgs(new Entities.Review
            {
                Id = Guid.Empty,
                Stars = 5,
                ReviewAuthor = new Author
                {
                    Id = Guid.Empty,
                    FirstName = "Lorem",
                    LastName = "Ipsum"
                },
                ReviewedMovie = new Movie
                {
                    Id = Guid.Empty,
                    Title = "Lorem Ipsum"
                }
            });

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
            .ReturnsForAnyArgs(new Entities.Review
            {
                Id = Guid.Empty,
                Stars = 5,
                ReviewAuthor = new Author
                {
                    Id = Guid.Empty,
                    FirstName = "Lorem",
                    LastName = "Ipsum"
                },
                ReviewedMovie = new Movie
                {
                    Id = Guid.Empty,
                    Title = "Lorem Ipsum"
                }
            });

        // Act
        var response = await ReviewsEndpoints.CreateReview(request, mediator, httpRequest);

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
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await ReviewsEndpoints.CreateReview(request, mediator, httpRequest);

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
        var response = await ReviewsEndpoints.CreateReview(request, mediator, httpRequest);

        // Assert
        var result = response.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }
}


// namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

// using System.Collections.Generic;
// using System.Net;
// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;
// using Application.Authors.Entities;
// using Application.Reviews.Entities;
// using Application.Reviews.Entities;
// using Extensions;
// using Shouldly;
// using Xunit;

// public class ReviewEndpointTests
// {
//     private static readonly MinimalApiApplication Application = new();

//     [Fact]
//     public async Task CreateReview_ShouldReturn_Created()
//     {
//         // Arrange
//         using var client = Application.CreateClient();

//         using var authorResponse = await client.GetAsync("/api/authors");
//         var authorResult = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<List<Author>>()[0];
//         using var movieResponse = await client.GetAsync("/api/movies");
//         var movieResult = (await movieResponse.Content.ReadAsStringAsync()).Deserialize<List<Review>>()[0];
//         var json = (new { Stars = 5, AuthorId = authorResult.Id, ReviewId = movieResult.Id }).Serialize();
//         var content = new StringContent(json, Encoding.UTF8, "application/json");

//         // Act
//         using var response = await client.PostAsync("/api/reviews", content);
//         var result = (await response.Content.ReadAsStringAsync()).Deserialize<Review>();

//         // Assert
//         response.StatusCode.ShouldBe(HttpStatusCode.Created);

//         _ = result.ShouldNotBeNull();

//         _ = result.Id.ShouldBeOfType<Guid>();
//         _ = result.Stars.ShouldBeOfType<int>();
//         result.Stars.ShouldBe(5);

//         _ = result.ReviewAuthor.ShouldNotBeNull();
//         result.ReviewAuthor.Id.ShouldBe(authorResult.Id);
//         result.ReviewAuthor.FirstName.ShouldBe(authorResult.FirstName);
//         result.ReviewAuthor.LastName.ShouldBe(authorResult.LastName);

//         _ = result.ReviewedReview.ShouldNotBeNull();
//         result.ReviewedReview.Id.ShouldBe(movieResult.Id);
//         result.ReviewedReview.Title.ShouldBe(movieResult.Title);
//     }

//     [Fact]
//     public async Task DeleteReview_ShouldDelete_Review()
//     {
//         // Arrange
//         using var client = Application.CreateClient();
//         using var reviewResponse = await client.GetAsync("/api/reviews");
//         var reviewResult = (await reviewResponse.Content.ReadAsStringAsync()).Deserialize<List<Review>>()[0];

//         // Act
//         using var response = await client.DeleteAsync($"/api/reviews/{reviewResult.Id}");

//         // Assert
//         response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

//         (await client.GetAsync($"/api/reviews/{reviewResult.Id}")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
//     }

//     [Fact]
//     public async Task UpdateReview_ShouldReturn_NoContent()
//     {
//         // Arrange
//         using var client = Application.CreateClient();
//         using var authorResponse = await client.GetAsync("/api/authors");
//         var authorResult = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<List<Author>>()[0];
//         using var movieResponse = await client.GetAsync("/api/movies");
//         var movieResult = (await movieResponse.Content.ReadAsStringAsync()).Deserialize<List<Review>>()[0];
//         using var reviewResponse = await client.GetAsync("/api/reviews");
//         var reviewResult = (await reviewResponse.Content.ReadAsStringAsync()).Deserialize<List<Review>>()[0];
//         var json = (new { Stars = 5, AuthorId = authorResult.Id, ReviewId = movieResult.Id }).Serialize();
//         var content = new StringContent(json, Encoding.UTF8, "application/json");

//         // Act
//         using var response = await client.PutAsync($"/api/reviews/{reviewResult.Id}", content);

//         // Assert
//         response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

//         using var validateResponse = await client.GetAsync($"/api/reviews/{reviewResult.Id}");
//         var validateResult = (await validateResponse.Content.ReadAsStringAsync()).Deserialize<Review>();

//         _ = validateResult.ShouldNotBeNull();

//         validateResult.Id.ShouldBe(reviewResult.Id);
//         validateResult.Stars.ShouldBe(5);

//         _ = validateResult.ReviewAuthor.ShouldNotBeNull();
//         validateResult.ReviewAuthor.Id.ShouldBe(authorResult.Id);
//         validateResult.ReviewAuthor.FirstName.ShouldBe(authorResult.FirstName);
//         validateResult.ReviewAuthor.LastName.ShouldBe(authorResult.LastName);

//         _ = validateResult.ReviewedReview.ShouldNotBeNull();
//         validateResult.ReviewedReview.Id.ShouldBe(movieResult.Id);
//         validateResult.ReviewedReview.Title.ShouldBe(movieResult.Title);
//     }
// }

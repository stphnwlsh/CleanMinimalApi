namespace CleanMinimalApi.Presentation.Tests.Integration.Endpoints;

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Authors.Entities;
using Application.Common.Paging;
using Application.Movies.Entities;
using Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;
using Entities = Application.Reviews.Entities;

public class ReviewEndpointTests : IDisposable
{
    private readonly MinimalApiApplication application;

    public ReviewEndpointTests()
    {
        this.application = new();
    }

    [Fact]
    public async Task CreateReview_ShouldReturn_Created()
    {
        // Arrange
        using var client = this.application.CreateClient();

        using var authorResponse = await client.GetAsync("/api/authors");
        var authorResult = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<Page<Author>>().Items[0];
        using var movieResponse = await client.GetAsync("/api/movies");
        var movieResult = (await movieResponse.Content.ReadAsStringAsync()).Deserialize<Page<Movie>>().Items[0];
        var json = (new { Stars = 5, AuthorId = authorResult.Id, MovieId = movieResult.Id }).Serialize();
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PostAsync("/api/reviews", content);
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Entities.Review>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull().ToString().ShouldEndWith($"/api/reviews/{result.Id}");

        _ = result.ShouldNotBeNull();

        _ = result.Id.ShouldBeOfType<Guid>();
        _ = result.Stars.ShouldBeOfType<int>();
        result.Stars.ShouldBe(5);

        _ = result.ReviewAuthor.ShouldNotBeNull();
        result.ReviewAuthor.Id.ShouldBe(authorResult.Id);
        result.ReviewAuthor.FirstName.ShouldBe(authorResult.FirstName);
        result.ReviewAuthor.LastName.ShouldBe(authorResult.LastName);

        _ = result.ReviewedMovie.ShouldNotBeNull();
        result.ReviewedMovie.Id.ShouldBe(movieResult.Id);
        result.ReviewedMovie.Title.ShouldBe(movieResult.Title);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_NoContent()
    {
        // Arrange
        using var client = this.application.CreateClient();
        using var reviewResponse = await client.GetAsync("/api/reviews?limit=50");
        var reviewResult = (await reviewResponse.Content.ReadAsStringAsync())
            .Deserialize<Page<Entities.Review>>().Items[0];

        // Act
        using var response = await client.DeleteAsync($"/api/reviews/{reviewResult.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        (await client.GetAsync($"/api/reviews/{reviewResult.Id}")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_Ok()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync("/api/reviews?limit=50");
        var result = (await response.Content.ReadAsStringAsync())
            .Deserialize<Page<Entities.Review>>().Items;

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(50);

        foreach (var review in result)
        {
            _ = review.ShouldNotBeNull();
            _ = review.Id.ShouldBeOfType<Guid>();
            _ = review.Stars.ShouldBeOfType<int>();
            review.Stars.ShouldBeInRange(1, 5);

            _ = review.ReviewAuthor.ShouldNotBeNull();
            _ = review.ReviewAuthor!.Id.ShouldBeOfType<Guid>();
            _ = review.ReviewAuthor!.FirstName.ShouldBeOfType<string>();
            review.ReviewAuthor!.FirstName.ShouldNotBeNullOrWhiteSpace();
            _ = review.ReviewAuthor!.LastName.ShouldBeOfType<string>();
            review.ReviewAuthor!.LastName.ShouldNotBeNullOrWhiteSpace();

            _ = review.ReviewedMovie.ShouldNotBeNull();
            _ = review.ReviewedMovie!.Id.ShouldBeOfType<Guid>();
            _ = review.ReviewedMovie!.Title.ShouldBeOfType<string>();
            review.ReviewedMovie!.Title.ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task GetReviewById_ShouldReturn_Ok()
    {
        // Arrange
        using var client = this.application.CreateClient();
        using var reviewResponse = await client.GetAsync("/api/reviews?limit=50");
        var reviewResult = (await reviewResponse.Content.ReadAsStringAsync())
            .Deserialize<Page<Entities.Review>>().Items[0];

        // Act
        using var response = await client.GetAsync($"/api/reviews/{reviewResult.Id}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Entities.Review>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        _ = result.ShouldNotBeNull();
        _ = result.Id.ShouldBeOfType<Guid>();
        result.Id.ShouldBe(reviewResult.Id);
        _ = result.Stars.ShouldBeOfType<int>();
        result.Stars.ShouldBeInRange(1, 5);

        _ = result.ReviewAuthor.ShouldNotBeNull();
        _ = result.ReviewAuthor.Id.ShouldBeOfType<Guid>();
        _ = result.ReviewAuthor.FirstName.ShouldBeOfType<string>();
        result.ReviewAuthor.FirstName.ShouldNotBeNullOrWhiteSpace();
        _ = result.ReviewAuthor.LastName.ShouldBeOfType<string>();
        result.ReviewAuthor.LastName.ShouldNotBeNullOrWhiteSpace();

        _ = result.ReviewedMovie.ShouldNotBeNull();
        _ = result.ReviewedMovie.Id.ShouldBeOfType<Guid>();
        _ = result.ReviewedMovie.Title.ShouldBeOfType<string>();
    }

    [Fact]
    public async Task GetReviewId_ShouldReturn_ValidationProblem()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/api/reviews/{Guid.Empty}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ValidationProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        _ = result.ShouldNotBeNull();

        result.Status.ShouldBe(StatusCodes.Status400BadRequest);
        result.Errors.ShouldContainKey("Id");
        result.Errors["Id"].ShouldBe(["A Review Id was not supplied."]);
    }

    [Fact]
    public async Task GetReviewId_ShouldReturn_NotFound()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/api/reviews/{Guid.NewGuid()}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        _ = result.ShouldNotBeNull();

        result.Status.ShouldBe(StatusCodes.Status404NotFound);
        result.Detail.ShouldBe("The Review with the supplied id was not found.");
    }

    [Theory]
    [InlineData("1")]
    [InlineData("fake")]
    public async Task GetReviewId_ShouldReturn_NotFound_WhenIdIsNotAGuid(string input)
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/api/reviews/{input}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        _ = result.ShouldNotBeNull();

        result.Status.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task CreateReview_ShouldReturn_ValidationProblem()
    {
        // Arrange
        using var client = this.application.CreateClient();
        var json = (new { Stars = 0, AuthorId = Guid.Empty, MovieId = Guid.Empty }).Serialize();
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PostAsync("/api/reviews", content);
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ValidationProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        _ = result.ShouldNotBeNull();

        result.Errors["AuthorId"].ShouldBe(["An author id was not supplied to create the review."]);
        result.Errors["MovieId"].ShouldBe(["A movie id was not supplied to create the review."]);
        result.Errors["Stars"].ShouldBe(["A star rating must be between 1 and 5."]);
    }

    [Fact]
    public async Task CreateReview_ShouldReturn_NotFound()
    {
        // Arrange
        using var client = this.application.CreateClient();
        var json = (new { Stars = 5, AuthorId = Guid.NewGuid(), MovieId = Guid.NewGuid() }).Serialize();
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PostAsync("/api/reviews", content);
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        _ = result.ShouldNotBeNull();

        result.Detail.ShouldBe("The Author with the supplied id was not found.");
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_NoContent()
    {
        // Arrange
        using var client = this.application.CreateClient();

        using var reviewResponse = await client.GetAsync("/api/reviews?limit=50");
        var reviewResult = (await reviewResponse.Content.ReadAsStringAsync())
            .Deserialize<Page<Entities.Review>>().Items[0];

        var stars = reviewResult.Stars == 5 ? 1 : 5;
        var json = (new { Stars = stars }).Serialize();
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PutAsync($"/api/reviews/{reviewResult.Id}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        using var validateResponse = await client.GetAsync($"/api/reviews/{reviewResult.Id}");
        var validateResult = (await validateResponse.Content.ReadAsStringAsync()).Deserialize<Entities.Review>();

        _ = validateResult.ShouldNotBeNull();

        validateResult.Id.ShouldBe(reviewResult.Id);
        validateResult.Stars.ShouldBe(stars);
        validateResult.ReviewAuthor.ShouldBe(reviewResult.ReviewAuthor);
        validateResult.ReviewedMovie.ShouldBe(reviewResult.ReviewedMovie);
    }

    [Fact]
    public async Task UpdateReview_ShouldNotChange_AuthorOrMovie()
    {
        // Arrange
        using var client = this.application.CreateClient();

        using var reviewResponse = await client.GetAsync("/api/reviews?limit=50");
        var reviewResult = (await reviewResponse.Content.ReadAsStringAsync())
            .Deserialize<Page<Entities.Review>>().Items[0];

        var json = (new { Stars = 3, AuthorId = Guid.NewGuid(), MovieId = Guid.NewGuid() }).Serialize();
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PutAsync($"/api/reviews/{reviewResult.Id}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        using var validateResponse = await client.GetAsync($"/api/reviews/{reviewResult.Id}");
        var validateResult = (await validateResponse.Content.ReadAsStringAsync()).Deserialize<Entities.Review>();

        validateResult.Stars.ShouldBe(3);
        validateResult.ReviewAuthor.ShouldBe(reviewResult.ReviewAuthor);
        validateResult.ReviewedMovie.ShouldBe(reviewResult.ReviewedMovie);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_NotFound()
    {
        // Arrange
        using var client = this.application.CreateClient();
        using var content = new StringContent((new { Stars = 3 }).Serialize(), Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PutAsync($"/api/reviews/{Guid.NewGuid()}", content);
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        result.Detail.ShouldBe("The Review with the supplied id was not found.");
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_FirstPage()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync("/api/reviews");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Page<Entities.Review>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.Items.Count.ShouldBe(Paging.DefaultLimit);
        result.TotalCount.ShouldBe(150);
        result.Offset.ShouldBe(0);
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_ReviewsOfMovie()
    {
        // Arrange
        using var client = this.application.CreateClient();
        using var reviewsResponse = await client.GetAsync("/api/reviews");
        var reviews = (await reviewsResponse.Content.ReadAsStringAsync()).Deserialize<Page<Entities.Review>>().Items;
        var movieId = reviews.First().ReviewedMovie!.Id;

        // Act
        using var response = await client.GetAsync($"/api/reviews?movieId={movieId}&limit=50");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Page<Entities.Review>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.TotalCount.ShouldBe(result.Items.Count);
        result.Items.ShouldNotBeEmpty();
        result.Items.ShouldAllBe(r => r.ReviewedMovie!.Id == movieId);
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_ReviewsByAuthor()
    {
        // Arrange
        using var client = this.application.CreateClient();
        using var reviewsResponse = await client.GetAsync("/api/reviews");
        var reviews = (await reviewsResponse.Content.ReadAsStringAsync()).Deserialize<Page<Entities.Review>>().Items;
        var authorId = reviews.First().ReviewAuthor!.Id;

        // Act
        using var response = await client.GetAsync($"/api/reviews?authorId={authorId}&limit=50");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Page<Entities.Review>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.TotalCount.ShouldBe(result.Items.Count);
        result.Items.ShouldNotBeEmpty();
        result.Items.ShouldAllBe(r => r.ReviewAuthor!.Id == authorId);
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_ValidationProblem()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/api/reviews?movieId={Guid.Empty}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ValidationProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        _ = result.ShouldNotBeNull();

        result.Status.ShouldBe(StatusCodes.Status400BadRequest);
        result.Errors.ShouldContainKey("MovieId");
        result.Errors["MovieId"].ShouldBe(["A Movie Id can't be empty. Leave it out to list every movie."]);
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.application.Dispose();
        }
    }
}

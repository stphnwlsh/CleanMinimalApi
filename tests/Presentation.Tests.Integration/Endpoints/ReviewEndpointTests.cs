namespace CleanMinimalApi.Presentation.Tests.Integration.Endpoints;

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Authors.Entities;
using Application.Movies.Entities;
using Application.Reviews.Entities;
using Extensions;
using Shouldly;
using Xunit;

public class ReviewEndpointTests
{
    private static readonly MinimalApiApplication Application = new();

    [Fact]
    public async Task CreateReview_ShouldReturn_Created()
    {
        // Arrange
        using var client = Application.CreateClient();

        using var authorResponse = await client.GetAsync("/api/authors");
        var authorResult = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<List<Author>>()[0];
        using var movieResponse = await client.GetAsync("/api/movies");
        var movieResult = (await movieResponse.Content.ReadAsStringAsync()).Deserialize<List<Movie>>()[0];
        var json = (new { Stars = 5, AuthorId = authorResult.Id, MovieId = movieResult.Id }).Serialize();
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PostAsync("/api/reviews", content);
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Review>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

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
    public async Task DeleteReview_ShouldDelete_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var reviewResponse = await client.GetAsync("/api/reviews");
        var reviewResult = (await reviewResponse.Content.ReadAsStringAsync()).Deserialize<List<Review>>()[0];

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
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/api/reviews");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<List<Review>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(150);

        foreach (var review in result)
        {
            _ = review.ShouldNotBeNull();
            _ = review.Id.ShouldBeOfType<Guid>();
            _ = review.Stars.ShouldBeOfType<int>();
            review.Stars.ShouldBeInRange(1, 5);

            _ = review.ReviewAuthor.ShouldNotBeNull();
            _ = review.ReviewAuthor.Id.ShouldBeOfType<Guid>();
            _ = review.ReviewAuthor.FirstName.ShouldBeOfType<string>();
            review.ReviewAuthor.FirstName.ShouldNotBeNullOrWhiteSpace();
            _ = review.ReviewAuthor.LastName.ShouldBeOfType<string>();
            review.ReviewAuthor.LastName.ShouldNotBeNullOrWhiteSpace();

            _ = review.ReviewedMovie.ShouldNotBeNull();
            _ = review.ReviewedMovie.Id.ShouldBeOfType<Guid>();
            _ = review.ReviewedMovie.Title.ShouldBeOfType<string>();
            review.ReviewedMovie.Title.ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task GetReviewById_ShouldReturn_Ok()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var reviewResponse = await client.GetAsync("/api/reviews");
        var reviewResult = (await reviewResponse.Content.ReadAsStringAsync()).Deserialize<List<Review>>()[0];

        // Act
        using var response = await client.GetAsync($"/api/reviews/{reviewResult.Id}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Review>();

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
    public async Task UpdateReview_ShouldReturn_NoContent()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var authorResponse = await client.GetAsync("/api/authors");
        var authorResult = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<List<Author>>()[0];
        using var movieResponse = await client.GetAsync("/api/movies");
        var movieResult = (await movieResponse.Content.ReadAsStringAsync()).Deserialize<List<Movie>>()[0];
        using var reviewResponse = await client.GetAsync("/api/reviews");
        var reviewResult = (await reviewResponse.Content.ReadAsStringAsync()).Deserialize<List<Review>>()[0];
        var json = (new { Stars = 5, AuthorId = authorResult.Id, MovieId = movieResult.Id }).Serialize();
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PutAsync($"/api/reviews/{reviewResult.Id}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        using var validateResponse = await client.GetAsync($"/api/reviews/{reviewResult.Id}");
        var validateResult = (await validateResponse.Content.ReadAsStringAsync()).Deserialize<Review>();

        _ = validateResult.ShouldNotBeNull();

        validateResult.Id.ShouldBe(reviewResult.Id);
        validateResult.Stars.ShouldBe(5);

        _ = validateResult.ReviewAuthor.ShouldNotBeNull();
        validateResult.ReviewAuthor.Id.ShouldBe(authorResult.Id);
        validateResult.ReviewAuthor.FirstName.ShouldBe(authorResult.FirstName);
        validateResult.ReviewAuthor.LastName.ShouldBe(authorResult.LastName);

        _ = validateResult.ReviewedMovie.ShouldNotBeNull();
        validateResult.ReviewedMovie.Id.ShouldBe(movieResult.Id);
        validateResult.ReviewedMovie.Title.ShouldBe(movieResult.Title);
    }
}

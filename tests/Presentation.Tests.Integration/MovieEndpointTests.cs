namespace CleanMinimalApi.Presentation.Tests.Integration;

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Entities;
using Shouldly;
using Xunit;

public class MovieEndpointTests
{
    private static readonly MinimalApiApplication Application = new();

    [Fact]
    public async Task ListMovies_ShouldReturn_Ok()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/movies");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<List<Movie>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(50);

        foreach (var movie in result)
        {
            _ = movie.ShouldNotBeNull();
            _ = movie.Id.ShouldBeOfType<Guid>();
            _ = movie.Title.ShouldBeOfType<string>();
            movie.Title.ShouldNotBeNullOrWhiteSpace();

            foreach (var review in movie.Reviews)
            {
                _ = review.Stars.ShouldBeOfType<int>();
                review.Stars.ShouldBeInRange(1, 5);

                _ = review.ReviewAuthor.ShouldNotBeNull();
                _ = review.ReviewAuthor.Id.ShouldBeOfType<Guid>();
                _ = review.ReviewAuthor.FirstName.ShouldBeOfType<string>();
                review.ReviewAuthor.FirstName.ShouldNotBeNullOrWhiteSpace();
                _ = review.ReviewAuthor.LastName.ShouldBeOfType<string>();
                review.ReviewAuthor.LastName.ShouldNotBeNullOrWhiteSpace();
            }
        }
    }

    [Fact]
    public async Task LookupMovie_ShouldReturn_Ok()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var authorResponse = await client.GetAsync("/movies");
        var authorResult = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<List<Movie>>()[0];

        // Act
        using var response = await client.GetAsync($"/movies/{authorResult.Id}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Movie>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.Id.ShouldBe(authorResult.Id);
        _ = result.Title.ShouldBeOfType<string>();
        result.Title.ShouldNotBeNullOrWhiteSpace();
        result.Reviews.ShouldNotBeEmpty();

        foreach (var review in result.Reviews)
        {
            _ = review.Stars.ShouldBeOfType<int>();
            review.Stars.ShouldBeInRange(1, 5);

            _ = review.ReviewAuthor.ShouldNotBeNull();
            _ = review.ReviewAuthor.Id.ShouldBeOfType<Guid>();
            _ = review.ReviewAuthor.FirstName.ShouldBeOfType<string>();
            review.ReviewAuthor.FirstName.ShouldNotBeNullOrWhiteSpace();
            _ = review.ReviewAuthor.LastName.ShouldBeOfType<string>();
            review.ReviewAuthor.LastName.ShouldNotBeNullOrWhiteSpace();
        }
    }
}

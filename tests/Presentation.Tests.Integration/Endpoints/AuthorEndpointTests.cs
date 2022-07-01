namespace CleanMinimalApi.Presentation.Tests.Integration.Endpoints;

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.Authors.Entities;
using Extensions;
using Shouldly;
using Xunit;

public class AuthorEndpointTests
{
    private static readonly MinimalApiApplication Application = new();

    [Fact]
    public async Task GetAuthors_ShouldReturn_Ok()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/api/authors");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<List<Author>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(15);

        foreach (var author in result)
        {
            _ = author.ShouldNotBeNull();
            _ = author.Id.ShouldBeOfType<Guid>();
            _ = author.FirstName.ShouldBeOfType<string>();
            author.FirstName.ShouldNotBeNullOrWhiteSpace();
            _ = author.LastName.ShouldBeOfType<string>();
            author.LastName.ShouldNotBeNullOrWhiteSpace();

            foreach (var review in author.Reviews)
            {
                _ = review.Stars.ShouldBeOfType<int>();
                review.Stars.ShouldBeInRange(1, 5);
                _ = review.ReviewedMovie.ShouldNotBeNull();
                _ = review.ReviewedMovie.Id.ShouldBeOfType<Guid>();
                _ = review.ReviewedMovie.Title.ShouldBeOfType<string>();
                review.ReviewedMovie.Title.ShouldNotBeNullOrWhiteSpace();
            }
        }
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_Ok()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var authorResponse = await client.GetAsync("/api/authors");
        var authorResult = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<List<Author>>()[0];

        // Act
        using var response = await client.GetAsync($"/api/authors/{authorResult.Id}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Author>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.Id.ShouldBe(authorResult.Id);
        _ = result.FirstName.ShouldBeOfType<string>();
        result.FirstName.ShouldNotBeNullOrWhiteSpace();
        _ = result.LastName.ShouldBeOfType<string>();
        result.FirstName.ShouldNotBeNullOrWhiteSpace();
        result.Reviews.ShouldNotBeEmpty();

        foreach (var review in result.Reviews)
        {
            _ = review.Stars.ShouldBeOfType<int>();
            review.Stars.ShouldBeInRange(1, 5);
            _ = review.ReviewedMovie.ShouldNotBeNull();
            _ = review.ReviewedMovie.Id.ShouldBeOfType<Guid>();
            _ = review.ReviewedMovie.Title.ShouldBeOfType<string>();
            review.ReviewedMovie.Title.ShouldNotBeNullOrWhiteSpace();
        }
    }
}

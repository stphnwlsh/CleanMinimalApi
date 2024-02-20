namespace CleanMinimalApi.Presentation.Tests.Integration.Endpoints;

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Extensions;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;
using Entities = Application.Movies.Entities;

public class MovieEndpointTests : IDisposable
{
    private MinimalApiApplication application;

    public MovieEndpointTests()
    {
        this.application = new();
    }

    [Fact]
    public async Task GetMovies_ShouldReturn_Ok()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync("/api/movie");
        var result = (await response.Content.ReadAsStringAsync())
            .Deserialize<List<Entities.Movie>>();

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
    public async Task GetMovieById_ShouldReturn_Ok()
    {
        // Arrange
        using var client = this.application.CreateClient();
        using var movieResponse = await client.GetAsync("/api/movie");
        var movieResult = (await movieResponse.Content.ReadAsStringAsync())
            .Deserialize<List<Entities.Movie>>()[0];

        // Act
        using var response = await client.GetAsync($"/api/movie/{movieResult.Id}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Entities.Movie>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.Id.ShouldBe(movieResult.Id);
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

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("1")]
    [InlineData("fake")]
    public async Task GetMovieId_ShouldReturn_ValidationProblem(string input)
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/api/movie/{input}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ValidationProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        _ = result.ShouldNotBeNull();

        result.Errors.ShouldNotBeEmpty();

        result.Errors.ShouldContainKey("");
        result.Errors[""].ShouldBe(["The Id supplied in the request is not valid."]);
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
            if (this.application != null)
            {
                this.application.Dispose();
                this.application = null;
            }
        }
    }
}

namespace CleanMinimalApi.Presentation.Tests.Integration.Endpoints;

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Extensions;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;
using Entities = Application.Authors.Entities;

public class AuthorEndpointTests : IDisposable
{
    private MinimalApiApplication application;

    public AuthorEndpointTests()
    {
        this.application = new();
    }

    [Fact]
    public async Task GetAuthors_ShouldReturn_Ok()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync("/api/author");
        var result = (await response.Content.ReadAsStringAsync())
            .Deserialize<List<Entities.Author>>();

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
        using var client = this.application.CreateClient();
        using var authorResponse = await client.GetAsync("/api/author");
        var authorResult = (await authorResponse.Content.ReadAsStringAsync())
            .Deserialize<List<Entities.Author>>()[0];

        // Act
        using var response = await client.GetAsync($"/api/author/{authorResult.Id}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Entities.Author>();

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

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("1")]
    [InlineData("fake")]
    public async Task GetAuthorById_ShouldReturn_ValidationProblem(string input)
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/api/author/{input}");
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

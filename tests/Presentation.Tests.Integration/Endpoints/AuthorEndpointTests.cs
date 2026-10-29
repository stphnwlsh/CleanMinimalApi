namespace CleanMinimalApi.Presentation.Tests.Integration.Endpoints;

using System.Net;
using System.Threading.Tasks;
using Application.Common.Paging;
using Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;
using Entities = Application.Authors.Entities;

public class AuthorEndpointTests : IDisposable
{
    private readonly MinimalApiApplication application;

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
        using var response = await client.GetAsync("/api/authors?limit=50");
        var result = (await response.Content.ReadAsStringAsync())
            .Deserialize<Page<Entities.AuthorSummary>>().Items;

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

            author.ReviewCount.ShouldBeGreaterThanOrEqualTo(0);
            (author.AverageStars is null || author.AverageStars is >= 1 and <= 5).ShouldBeTrue();
            (author.AverageStars is null).ShouldBe(author.ReviewCount == 0);
        }
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_Ok()
    {
        // Arrange
        using var client = this.application.CreateClient();
        using var authorResponse = await client.GetAsync("/api/authors?limit=50");
        var authorResult = (await authorResponse.Content.ReadAsStringAsync())
            .Deserialize<Page<Entities.AuthorSummary>>().Items.First(a => a.ReviewCount > 0);

        // Act
        using var response = await client.GetAsync($"/api/authors/{authorResult.Id}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Entities.Author>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.Id.ShouldBe(authorResult.Id);
        _ = result.FirstName.ShouldBeOfType<string>();
        result.FirstName.ShouldNotBeNullOrWhiteSpace();
        _ = result.LastName.ShouldBeOfType<string>();
        result.FirstName.ShouldNotBeNullOrWhiteSpace();
        result.Reviews.Count.ShouldBe(authorResult.ReviewCount);

        foreach (var review in result.Reviews.ShouldNotBeNull())
        {
            _ = review.Stars.ShouldBeOfType<int>();
            review.Stars.ShouldBeInRange(1, 5);
            _ = review.ReviewedMovie.ShouldNotBeNull();
            _ = review.ReviewedMovie!.Id.ShouldBeOfType<Guid>();
            _ = review.ReviewedMovie!.Title.ShouldBeOfType<string>();
            review.ReviewedMovie!.Title.ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_ValidationProblem()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/api/authors/{Guid.Empty}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ValidationProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        _ = result.ShouldNotBeNull();

        result.Status.ShouldBe(StatusCodes.Status400BadRequest);
        result.Errors.ShouldContainKey("Id");
        result.Errors["Id"].ShouldBe(["An Author Id was not supplied."]);
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_NotFound()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/api/authors/{Guid.NewGuid()}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        _ = result.ShouldNotBeNull();

        result.Status.ShouldBe(StatusCodes.Status404NotFound);
        result.Detail.ShouldBe("The Author with the supplied id was not found.");
    }

    [Theory]
    [InlineData("1")]
    [InlineData("fake")]
    public async Task GetAuthorById_ShouldReturn_NotFound_WhenIdIsNotAGuid(string input)
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/api/authors/{input}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        _ = result.ShouldNotBeNull();

        result.Status.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task GetAuthors_ShouldReturn_FirstPage()
    {
        // Arrange
        using var client = this.application.CreateClient();
        using var authorsResponse = await client.GetAsync("/api/authors");
        var authors = (await authorsResponse.Content.ReadAsStringAsync()).Deserialize<Page<Entities.AuthorSummary>>().Items;

        // Act
        using var response = await client.GetAsync("/api/authors?limit=5");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Page<Entities.AuthorSummary>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.Items.Count.ShouldBe(Math.Min(5, authors.Count));
        result.TotalCount.ShouldBe(15);
    }

    [Fact]
    public async Task GetAuthors_ShouldReturn_Matches()
    {
        // Arrange
        using var client = this.application.CreateClient();
        using var authorsResponse = await client.GetAsync("/api/authors?limit=50");
        var lastName = (await authorsResponse.Content.ReadAsStringAsync())
            .Deserialize<Page<Entities.AuthorSummary>>().Items
            .First().LastName;

        // Act
        using var response = await client.GetAsync($"/api/authors?query={Uri.EscapeDataString(lastName.ToLowerInvariant())}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Page<Entities.AuthorSummary>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.Items.ShouldNotBeEmpty();
        result.Items.ShouldAllBe(a => $"{a.FirstName} {a.LastName}".Contains(lastName, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetAuthors_ShouldReturn_ValidationProblem()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/api/authors?query={new string('a', 101)}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ValidationProblemDetails>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        _ = result.ShouldNotBeNull();

        result.Status.ShouldBe(StatusCodes.Status400BadRequest);
        result.Errors.ShouldContainKey("Query");
        result.Errors["Query"].ShouldBe(["A search term can't be longer than 100 characters."]);
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

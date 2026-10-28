namespace CleanMinimalApi.Presentation.Tests.Integration.OpenApi;

using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

public class OpenApiDocumentTests : IDisposable
{
    private readonly MinimalApiApplication application;

    public OpenApiDocumentTests()
    {
        this.application = new();
    }

    [Fact]
    public async Task GetOpenApiDocument_ShouldDescribe_Api()
    {
        // Arrange
        using var document = await this.GetOpenApiDocument();

        // Act
        var info = document.RootElement.GetProperty("info");

        // Assert
        info.GetProperty("title").GetString()!.ShouldStartWith("CleanMinimalApi API");
        info.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();
        info.GetProperty("license").GetProperty("name").GetString().ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetOpenApiDocument_ShouldDescribe_Tags()
    {
        // Arrange
        using var document = await this.GetOpenApiDocument();

        // Act
        var tags = document.RootElement.GetProperty("tags").EnumerateArray().ToList();

        // Assert
        tags.Select(t => t.GetProperty("name").GetString()).ShouldBe(["Authors", "Movies", "Reviews", "Version"], ignoreOrder: true);

        foreach (var tag in tags)
        {
            tag.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task GetOpenApiDocument_ShouldDescribe_EveryOperation()
    {
        // Arrange
        using var document = await this.GetOpenApiDocument();

        // Act
        var operations = document.RootElement.GetProperty("paths").EnumerateObject()
            .SelectMany(p => p.Value.EnumerateObject())
            .Select(o => o.Value)
            .ToList();

        // Assert
        operations.Count.ShouldBe(10);

        foreach (var operation in operations)
        {
            operation.GetProperty("operationId").GetString().ShouldNotBeNullOrWhiteSpace();
            operation.GetProperty("summary").GetString().ShouldNotBeNullOrWhiteSpace();
            operation.GetProperty("description").GetString()!.ShouldContain("Sample request:");

            if (operation.TryGetProperty("parameters", out var parameters))
            {
                foreach (var parameter in parameters.EnumerateArray())
                {
                    parameter.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();
                }
            }

            if (operation.TryGetProperty("requestBody", out var requestBody))
            {
                requestBody.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();
            }

            foreach (var response in operation.GetProperty("responses").EnumerateObject())
            {
                response.Value.GetProperty("description").GetString()!.ShouldEndWith(".");
            }
        }
    }

    [Theory]
    [InlineData("Author")]
    [InlineData("Movie")]
    [InlineData("Review")]
    [InlineData("Version")]
    [InlineData("PageOfAuthor")]
    [InlineData("PageOfMovie")]
    [InlineData("PageOfReview")]
    [InlineData("CreateReviewRequest")]
    [InlineData("UpdateReviewRequest")]
    public async Task GetOpenApiDocument_ShouldDescribe_Schema(string name)
    {
        // Arrange
        using var document = await this.GetOpenApiDocument();

        // Act
        var schema = document.RootElement.GetProperty("components").GetProperty("schemas").GetProperty(name);

        // Assert
        schema.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();

        foreach (var property in schema.GetProperty("properties").EnumerateObject())
        {
            Description(property.Value).ShouldNotBeNullOrWhiteSpace();
        }
    }

    // A nullable reference to another schema is written as oneOf null or the schema, with the description on the schema.
    private static string? Description(JsonElement property)
    {
        if (property.TryGetProperty("description", out var description))
        {
            return description.GetString();
        }

        return property.GetProperty("oneOf").EnumerateArray()
            .Select(option => option.TryGetProperty("description", out var value) ? value.GetString() : null)
            .FirstOrDefault(value => value is not null);
    }

    [Fact]
    public async Task GetScalar_ShouldReturn_Ok()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync("/scalar");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.ShouldBe("text/html");
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

    private async Task<JsonDocument> GetOpenApiDocument()
    {
        using var client = this.application.CreateClient();
        using var response = await client.GetAsync("/openapi/v1.json");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        return JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    }
}

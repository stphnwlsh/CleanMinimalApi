namespace CleanMinimalApi.Presentation.Tests.Integration;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CleanMinimalApi.Domain.Entities.Notes;
using Shouldly;
using Xunit;

[ExcludeFromCodeCoverage]
public class NotesEndpointTests
{
    private static readonly MinimalApiApplication Application = new();

    [Fact]
    public async Task CreateNote_ShouldReturn_Created()
    {
        // Arrange
        using var client = Application.CreateClient();
        var json = (new { Text = "Meaning of Life" }).Serialize();
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PostAsync("/notes", content);
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Note>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        _ = result.ShouldNotBeNull();

        _ = result.Id.ShouldBeOfType<int>();
        result.Id.ShouldBeInRange(0, 100);
        _ = result.Text.ShouldBeOfType<string>();
        result.Text.ShouldNotBe(default);
    }

    [Fact]
    public async Task DeleteNote_ShouldReturn_NoContent()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.DeleteAsync("/notes/42");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }


    [Fact]
    public async Task ListNotes_ShouldReturn_Ok()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/notes");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<List<Note>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(3);

        _ = result[0].ShouldNotBeNull();
        _ = result[0].ShouldBeOfType<Note>();
        _ = result[0].Id.ShouldBeOfType<int>();
        result[0].Id.ShouldBeInRange(0, 100);
        _ = result[0].Text.ShouldBeOfType<string>();
        result[0].Text.ShouldNotBe(default);

        _ = result[1].ShouldNotBeNull();
        _ = result[1].ShouldBeOfType<Note>();
        _ = result[1].Id.ShouldBeOfType<int>();
        result[1].Id.ShouldBeInRange(0, 100);
        _ = result[1].Text.ShouldBeOfType<string>();
        result[1].Text.ShouldNotBe(default);

        _ = result[2].ShouldNotBeNull();
        _ = result[2].ShouldBeOfType<Note>();
        _ = result[2].Id.ShouldBeOfType<int>();
        result[2].Id.ShouldBeInRange(0, 100);
        _ = result[2].Text.ShouldBeOfType<string>();
        result[2].Text.ShouldNotBe(default);
    }

    [Fact]
    public async Task LookupNote_ShouldReturn_Ok()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/notes/42");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Note>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        _ = result.Id.ShouldBeOfType<int>();
        result.Id.ShouldBeInRange(0, 100);
        _ = result.Text.ShouldBeOfType<string>();
        result.Text.ShouldNotBe(default);
    }

    [Fact]
    public async Task UpdateNote_ShouldReturn_NoContent()
    {
        // Arrange
        using var client = Application.CreateClient();
        var json = (new { Text = "Meaning of Life" }).Serialize();
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PutAsync("/notes/42", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}

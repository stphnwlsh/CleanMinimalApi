namespace CleanMinimalApi.Presentation.Tests.Integration;
using System.Net;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Entities;
using Shouldly;
using Xunit;

public class VersionEndpointTests
{
    private static readonly MinimalApiApplication Application = new();

    [Fact]
    public async Task Version_ShouldReturn_Ok()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/version");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<ApplicationVersion>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        _ = result.FileVersion.ShouldBeOfType<string>();
        result.FileVersion.ShouldNotBeNullOrWhiteSpace();
        _ = result.InformationalVersion.ShouldBeOfType<string>();
        result.InformationalVersion.ShouldNotBeNullOrWhiteSpace();
    }
}

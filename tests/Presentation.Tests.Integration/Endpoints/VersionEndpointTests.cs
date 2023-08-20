namespace CleanMinimalApi.Presentation.Tests.Integration.Endpoints;

using System.Net;
using System.Threading.Tasks;
using Extensions;
using Shouldly;
using Xunit;
using Entities = Application.Versions.Entities;

public class VersionEndpointTests : IDisposable
{
    private MinimalApiApplication application;

    public VersionEndpointTests()
    {
        this.application = new();
    }

    [Fact]
    public async Task GetVersion_ShouldReturn_Ok()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync("/api/version");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<Entities.Version>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<Entities.Version>();

        _ = result.FileVersion.ShouldBeOfType<string>();
        result.FileVersion.ShouldNotBeNullOrWhiteSpace();
        _ = result.InformationalVersion.ShouldBeOfType<string>();
        result.InformationalVersion.ShouldNotBeNullOrWhiteSpace();
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

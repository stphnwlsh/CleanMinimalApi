namespace CleanMinimalApi.Presentation.Tests.Integration.Health;

using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

public class HealthEndpointTests : IDisposable
{
    private readonly MinimalApiApplication application = new(settings: new Dictionary<string, string?>
    {
        // Health checks have to answer without a token, so they're tested with authentication on.
        ["Authentication:Authority"] = "https://login.example.com",
        ["Authentication:Audience"] = "https://api.example.com"
    });

    [Theory]
    [InlineData("/health/live")]
    [InlineData("/health/ready")]
    public async Task GetHealth_ShouldReturn_Healthy(string path)
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync(path);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("Healthy");
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

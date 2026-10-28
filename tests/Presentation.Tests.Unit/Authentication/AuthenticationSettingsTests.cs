namespace CleanMinimalApi.Presentation.Tests.Unit.Authentication;

using CleanMinimalApi.Presentation.Authentication;
using Microsoft.Extensions.Configuration;
using Shouldly;
using Xunit;

public class AuthenticationSettingsTests
{
    [Fact]
    public void FromConfiguration_ShouldReturn_DisabledSettings_WhenSectionIsMissing()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act
        var result = AuthenticationSettings.FromConfiguration(configuration);

        // Assert
        result.Enabled.ShouldBeFalse();
    }

    [Fact]
    public void FromConfiguration_ShouldReturn_EnabledSettings_WhenAuthorityIsSet()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Authentication:Authority"] = "https://login.example.com",
                ["Authentication:Audience"] = "https://api.example.com",
            })
            .Build();

        // Act
        var result = AuthenticationSettings.FromConfiguration(configuration);

        // Assert
        result.Enabled.ShouldBeTrue();
        result.Authority.ShouldBe("https://login.example.com");
        result.Audience.ShouldBe("https://api.example.com");
    }
}

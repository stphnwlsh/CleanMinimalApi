namespace CleanMinimalApi.Presentation.Tests.Integration;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal sealed class MinimalApiApplication(
    string environment = "local",
    Dictionary<string, string?>? settings = null,
    Action<IServiceCollection>? configureServices = null) : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        _ = builder.UseEnvironment(environment);

        // Every application gets its own in-memory database, so tests never share or keep data.
        var testSettings = new Dictionary<string, string?>
        {
            ["ConnectionStrings:MovieReviews"] = $"Data Source=Tests-{Guid.NewGuid()};Mode=Memory;Cache=Shared",
            ["Database:SeedSampleData"] = "true"
        };

        foreach (var setting in settings ?? [])
        {
            testSettings[setting.Key] = setting.Value;
        }

        _ = builder.ConfigureHostConfiguration(configuration => configuration.AddInMemoryCollection(testSettings));

        if (configureServices is not null)
        {
            _ = builder.ConfigureServices(configureServices);
        }

        return base.CreateHost(builder);
    }
}

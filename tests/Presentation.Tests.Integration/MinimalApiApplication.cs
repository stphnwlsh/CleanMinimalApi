namespace CleanMinimalApi.Presentation.Tests.Integration;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal sealed class MinimalApiApplication(
    string environment = "local",
    Dictionary<string, string> settings = null,
    Action<IServiceCollection> configureServices = null) : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        _ = builder.UseEnvironment(environment);

        if (settings is not null)
        {
            _ = builder.ConfigureHostConfiguration(configuration => configuration.AddInMemoryCollection(settings));
        }

        if (configureServices is not null)
        {
            _ = builder.ConfigureServices(configureServices);
        }

        return base.CreateHost(builder);
    }
}

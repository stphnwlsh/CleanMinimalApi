namespace CleanMinimalApi.Presentation.Tests.Integration;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

internal sealed class MinimalApiApplication(string environment = "local") : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        _ = builder.UseEnvironment(environment);

        return base.CreateHost(builder);
    }
}

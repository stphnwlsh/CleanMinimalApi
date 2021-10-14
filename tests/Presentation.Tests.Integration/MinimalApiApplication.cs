namespace CleanMinimalApi.Presentation.Tests.Integration;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

[ExcludeFromCodeCoverage]
internal class MinimalApiApplication : WebApplicationFactory<Program>
{
    private readonly string environment;

    public MinimalApiApplication(string environment = "local")
    {
        this.environment = environment;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        _ = builder.UseEnvironment(this.environment);

        return base.CreateHost(builder);
    }
}

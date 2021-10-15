namespace CleanMinimalApi.Application.Versions.ApiVersion;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Domain.Entities.Versions;
using MediatR;

public class ApiVersionHandler : IRequestHandler<ApiVersionQuery, ApiVersion>
{
    public async Task<ApiVersion> Handle(ApiVersionQuery request, CancellationToken cancellationToken)
    {
        var version = new ApiVersion(
            $"{Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version}",
            $"{Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}"
        );

        return await Task.FromResult(version);
    }
}

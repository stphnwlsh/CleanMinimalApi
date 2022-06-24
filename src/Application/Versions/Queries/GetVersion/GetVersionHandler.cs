namespace CleanMinimalApi.Application.Versions.Queries.GetVersion;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Entities;
using MediatR;

public class GetVersionHandler : IRequestHandler<GetVersionQuery, ApplicationVersion>
{
    public async Task<ApplicationVersion> Handle(GetVersionQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new ApplicationVersion
        {
            FileVersion = $"{Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version}",
            InformationalVersion = $"{Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}"
        });
    }
}

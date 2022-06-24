namespace CleanMinimalApi.Application.Versions.Queries.GetVersion;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class GetVersionHandler : IRequestHandler<GetVersionQuery, Version>
{
    public async Task<Version> Handle(GetVersionQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new Version
        {
            FileVersion = $"{Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version}",
            InformationalVersion = $"{Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}"
        });
    }
}

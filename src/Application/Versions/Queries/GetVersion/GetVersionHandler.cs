namespace CleanMinimalApi.Application.Versions.Queries.GetVersion;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class GetVersionHandler : IRequestHandler<GetVersionQuery, Version>
{
    public Task<Version> Handle(GetVersionQuery request, CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetEntryAssembly();

        var version = new Version
        {
            FileVersion =
                $"{assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version}",
            InformationalVersion =
                $"{assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}"
        };

        return Task.FromResult(version);
    }
}

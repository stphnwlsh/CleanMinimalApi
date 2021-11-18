namespace CleanMinimalApi.Application.Versions.ReadVersion;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Domain.Version;
using MediatR;

public class ReadVersionHandler : IRequestHandler<ReadVersionQuery, ApplicationVersion>
{
    public async Task<ApplicationVersion> Handle(ReadVersionQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new ApplicationVersion
        {
            FileVersion = $"{Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version}",
            InformationalVersion = $"{Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}"
        });
    }
}

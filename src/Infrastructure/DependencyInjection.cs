namespace CleanMinimalApi.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Infrastructure.Persistance.HardCoded;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        _ = services.AddSingleton<HardCodedNotesDataSource>();
        _ = services.AddSingleton<INotesContext, HardCodedNotesContext>();

        return services;
    }
}

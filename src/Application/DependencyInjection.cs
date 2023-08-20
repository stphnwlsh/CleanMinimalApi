namespace CleanMinimalApi.Application;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        _ = services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}

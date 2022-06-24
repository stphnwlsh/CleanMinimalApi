namespace CleanMinimalApi.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using Application.Authors;
using Application.Movies;
using Application.Reviews;
using Databases.InMemoryMoviesReviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleDateTimeProvider;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        _ = services.AddEntityFrameworkInMemoryDatabase();
        _ = services.AddDbContext<MovieReviewsDbContext>(options => options.UseInMemoryDatabase($"Movies-{Guid.NewGuid()}"), ServiceLifetime.Singleton);

        _ = services.AddSingleton<MovieReviewsRepository>();
        _ = services.AddSingleton<IAuthorsRepository>(p => p.GetRequiredService<MovieReviewsRepository>());
        _ = services.AddSingleton<IMoviesRepository>(x => x.GetRequiredService<MovieReviewsRepository>());
        _ = services.AddSingleton<IReviewsRepository>(x => x.GetRequiredService<MovieReviewsRepository>());

        _ = services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}

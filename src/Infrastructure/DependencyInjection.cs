namespace CleanMinimalApi.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Infrastructure.Persistance.InMemory.MovieReviews;
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
        _ = services.AddSingleton<AuthorsRepository>(p => p.GetRequiredService<MovieReviewsRepository>());
        _ = services.AddSingleton<MoviesRepository>(x => x.GetRequiredService<MovieReviewsRepository>());
        _ = services.AddSingleton<ReviewsRepository>(x => x.GetRequiredService<MovieReviewsRepository>());

        _ = services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}

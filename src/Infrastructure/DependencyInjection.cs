namespace CleanMinimalApi.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CleanMinimalApi.Application.Authors;
using CleanMinimalApi.Application.Movies;
using CleanMinimalApi.Application.Reviews;
using CleanMinimalApi.Infrastructure.Databases.InMemoryMovieReviews;
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
        _ = services.AddSingleton<IAuthorsRepository>(x => x.GetRequiredService<MovieReviewsRepository>());
        _ = services.AddSingleton<IMoviesRepository>(x => x.GetRequiredService<MovieReviewsRepository>());
        _ = services.AddSingleton<IReviewsRepository>(x => x.GetRequiredService<MovieReviewsRepository>());

        _ = services.AddAutoMapper(Assembly.GetExecutingAssembly());

        _ = services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}

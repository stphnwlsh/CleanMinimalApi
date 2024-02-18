namespace CleanMinimalApi.Infrastructure;

using System;
using Application.Authors;
using Application.Movies;
using Application.Reviews;
using Databases.MoviesReviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        _ = services.AddDbContext<MovieReviewsDbContext>(options =>
            options.UseInMemoryDatabase($"Movies-{Guid.NewGuid()}"), ServiceLifetime.Singleton);

        _ = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        _ = services.AddSingleton<EntityFrameworkMovieReviewsRepository>();

        _ = services.AddSingleton<IAuthorsRepository>(p =>
            p.GetRequiredService<EntityFrameworkMovieReviewsRepository>());
        _ = services.AddSingleton<IMoviesRepository>(x =>
            x.GetRequiredService<EntityFrameworkMovieReviewsRepository>());
        _ = services.AddSingleton<IReviewsRepository>(x =>
            x.GetRequiredService<EntityFrameworkMovieReviewsRepository>());

        _ = services.AddSingleton(TimeProvider.System);

        return services;
    }
}

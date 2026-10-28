namespace CleanMinimalApi.Infrastructure;

using System;
using Application.Authors;
using Application.Movies;
using Application.Reviews;
using Databases.MovieReviews;
using Databases.MovieReviews.Extensions;
using Databases.MovieReviews.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    /// <summary>The connection string used when none is configured.</summary>
    public const string DefaultConnectionString = "Data Source=movie-reviews.db";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString = null)
    {
        var database = new MovieReviewsDatabase(string.IsNullOrWhiteSpace(connectionString) ? DefaultConnectionString : connectionString);

        _ = services.AddSingleton(database);
        _ = services.AddDbContext<MovieReviewsDbContext>(options => options.UseSqlite(database.ConnectionString));

        _ = services.AddSingleton<MovieReviewsMapper>();
        _ = services.AddScoped<EntityFrameworkMovieReviewsRepository>();

        _ = services.AddScoped<IAuthorsRepository>(p =>
            p.GetRequiredService<EntityFrameworkMovieReviewsRepository>());
        _ = services.AddScoped<IMoviesRepository>(x =>
            x.GetRequiredService<EntityFrameworkMovieReviewsRepository>());
        _ = services.AddScoped<IReviewsRepository>(x =>
            x.GetRequiredService<EntityFrameworkMovieReviewsRepository>());

        _ = services.AddSingleton(TimeProvider.System);

        return services;
    }

    /// <summary>
    /// Creates or upgrades the database with the migrations, then adds the sample data when it is empty.
    /// Call it once at startup, before the first request.
    /// </summary>
    public static IServiceProvider InitialiseInfrastructure(this IServiceProvider services)
    {
        services.GetRequiredService<MovieReviewsDatabase>().KeepAlive();

        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<MovieReviewsDbContext>();

        context.Database.Migrate();
        _ = context.AddData();

        return services;
    }
}

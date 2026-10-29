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
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
    /// Adds a readiness check that the movie reviews database can be reached.
    /// </summary>
    /// <param name="builder">The health checks builder.</param>
    /// <param name="tags">The tags to give the check, so endpoints can choose which checks they run.</param>
    public static IHealthChecksBuilder AddInfrastructureHealthChecks(this IHealthChecksBuilder builder, params string[] tags)
    {
        return builder.AddDbContextCheck<MovieReviewsDbContext>("MovieReviewsDatabase", HealthStatus.Unhealthy, tags);
    }

    /// <summary>
    /// Creates or upgrades the database with the migrations. Call it once at startup, before the first request.
    /// </summary>
    /// <param name="services">The application's services.</param>
    /// <param name="seedSampleData">
    /// Adds the sample authors, movies and reviews when the database is empty. Turn it on for local runs and demos,
    /// and leave it off anywhere real data lives.
    /// </param>
    public static IServiceProvider InitialiseInfrastructure(this IServiceProvider services, bool seedSampleData)
    {
        services.GetRequiredService<MovieReviewsDatabase>().KeepAlive();

        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<MovieReviewsDbContext>();

        context.Database.Migrate();

        if (seedSampleData)
        {
            _ = context.AddData();
        }

        return services;
    }
}

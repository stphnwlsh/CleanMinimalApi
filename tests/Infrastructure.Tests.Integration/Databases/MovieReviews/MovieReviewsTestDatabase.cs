namespace CleanMinimalApi.Infrastructure.Tests.Integration.Databases.MovieReviews;

using System;
using Extensions;
using Infrastructure.Databases.MovieReviews;
using Infrastructure.Databases.MovieReviews.Mapping;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

/// <summary>
/// A private in-memory database with the test data. Each test creates its own, so a test that changes the data
/// can't change what another test sees.
/// </summary>
public sealed class MovieReviewsTestDatabase : IDisposable
{
    private SqliteConnection? connection;

    internal MovieReviewsDbContext Context { get; set; }
    internal FakeTimeProvider TimeProvider { get; set; }
    internal EntityFrameworkMovieReviewsRepository Repository { get; set; }

    public MovieReviewsTestDatabase()
    {
        // A private in-memory SQLite database that lives as long as this connection is open,
        // so every test starts from the same data and nothing is written to disk.
        this.connection = new SqliteConnection("Data Source=:memory:");
        this.connection.Open();

        var options = new DbContextOptionsBuilder<MovieReviewsDbContext>()
            .UseSqlite(this.connection)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;

        this.Context = new MovieReviewsDbContext(options);

        this.TimeProvider = new FakeTimeProvider();
        this.TimeProvider.SetUtcNow(new DateTime(2009, 12, 31, 23, 51, 01, DateTimeKind.Utc));

        this.Repository = new EntityFrameworkMovieReviewsRepository(this.Context, this.TimeProvider, new MovieReviewsMapper());

        this.Context.Database.Migrate();
        _ = this.Context.AddTestData();
        this.Context.ChangeTracker.Clear();
    }

    public void Dispose()
    {
        this.Context.Dispose();
        this.connection?.Dispose();
        this.connection = null;
    }
}

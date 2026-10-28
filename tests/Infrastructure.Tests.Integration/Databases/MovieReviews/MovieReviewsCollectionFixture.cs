namespace CleanMinimalApi.Infrastructure.Tests.Integration.Databases.MovieReviews;

using System;
using Extensions;
using Infrastructure.Databases.MovieReviews;
using Infrastructure.Databases.MovieReviews.Mapping;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using Xunit;

[CollectionDefinition("MovieReviews")]
public class MovieReviewsCollectionFixture : ICollectionFixture<MovieReviewsDataFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class MovieReviewsDataFixture : IDisposable
{
    private SqliteConnection? connection;

    internal MovieReviewsDbContext Context { get; set; }
    internal FakeTimeProvider TimeProvider { get; set; }
    internal EntityFrameworkMovieReviewsRepository Repository { get; set; }

    public MovieReviewsDataFixture()
    {
        // A private in-memory SQLite database that lives as long as this connection is open,
        // so every test run starts from the same data and nothing is written to disk.
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
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.Context.Dispose();
            this.connection?.Dispose();
            this.connection = null;
        }
    }
}

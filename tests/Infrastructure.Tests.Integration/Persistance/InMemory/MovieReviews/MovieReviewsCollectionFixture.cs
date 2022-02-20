namespace CleanMinimalApi.Infrastructure.Tests.Integration.Persistance.InMemory.MovieReviews;

using System;
using CleanMinimalApi.Infrastructure.Persistance.InMemory.MovieReviews;
using Microsoft.EntityFrameworkCore;
using SimpleDateTimeProvider;
using Xunit;

[CollectionDefinition("MovieReviews")]
public class MovieReviewsCollectionFixture : ICollectionFixture<MovieReviewsDataFixture>
{
}

public class MovieReviewsDataFixture : IDisposable
{
    private bool disposedValue;

    internal MovieReviewsDbContext Context { get; set; }
    internal IDateTimeProvider DateTimeProvider { get; set; }
    internal MovieReviewsRepository Repository { get; set; }

    public MovieReviewsDataFixture()
    {
        var options = new DbContextOptionsBuilder<MovieReviewsDbContext>().UseInMemoryDatabase($"TestMovies-{Guid.NewGuid()}").Options;

        this.Context = new MovieReviewsDbContext(options);

        this.DateTimeProvider = new MockDateTimeProvider
        {
            Now = new DateTime(2000, 01, 01, 01, 01, 01),
            Today = new DateTime(2000, 01, 01, 00, 00, 00),
            UtcNow = new DateTime(1999, 12, 31, 23, 51, 01)
        };

        this.Repository = new MovieReviewsRepository(this.Context, this.DateTimeProvider);

        if (this.Context != null)
        {
            _ = this.Context.Database.EnsureDeleted();
            _ = this.Context.Database.EnsureCreated();
            _ = this.Context.AddTestData();
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.Context?.Dispose();
            }

            this.disposedValue = true;
        }
    }
}

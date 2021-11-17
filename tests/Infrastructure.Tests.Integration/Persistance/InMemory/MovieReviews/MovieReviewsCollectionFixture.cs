namespace CleanMinimalApi.Infrastructure.Tests.Integration.Persistance.InMemory.MovieReviews;

using System;
using CleanMinimalApi.Infrastructure.Persistance.InMemory.MovieReviews;
using Microsoft.EntityFrameworkCore;
using Xunit;

[CollectionDefinition("MovieReviews")]
public class MovieReviewsCollectionFixture : ICollectionFixture<MovieReviewsDataFixture>
{
}

public class MovieReviewsDataFixture : IDisposable
{
    private bool disposedValue;

    internal MovieReviewsDbContext? Context { get; set; }
    internal MovieReviewsRepository? Repository { get; set; }

    public MovieReviewsDataFixture()
    {
        var options = new DbContextOptionsBuilder<MovieReviewsDbContext>().UseInMemoryDatabase($"TestMovies-{Guid.NewGuid()}").Options;

        this.Context = new MovieReviewsDbContext(options);
        this.Repository = new MovieReviewsRepository(this.Context);

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

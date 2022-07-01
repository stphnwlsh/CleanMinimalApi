namespace CleanMinimalApi.Infrastructure.Tests.Integration.Databases.MovieReviews;

using System;
using AutoMapper;
using Extensions;
using Infrastructure.Databases.MoviesReviews;
using Infrastructure.Databases.MoviesReviews.Mapping;
using Microsoft.EntityFrameworkCore;
using SimpleDateTimeProvider;
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
    private bool disposedValue;

    internal MovieReviewsDbContext Context { get; set; }
    internal IDateTimeProvider DateTimeProvider { get; set; }
    internal IMapper Mapper { get; set; }
    internal EntityFrameworkMovieReviewsRepository Repository { get; set; }

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

        this.Mapper = new MapperConfiguration(cfg =>
            cfg
                .AddProfiles(new List<Profile>()
                {
                    new AuthorMappingProfile(),
                    new EntitiyMappingProfile(),
                    new MovieMappingProfile(),
                    new ReviewMappingProfile()
                }))
                .CreateMapper();

        this.Repository = new EntityFrameworkMovieReviewsRepository(this.Context, this.DateTimeProvider, this.Mapper);

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

namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Models;

internal class MovieReviewsDbContext(DbContextOptions<MovieReviewsDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors => this.Set<Author>();
    public DbSet<Movie> Movies => this.Set<Movie>();
    public DbSet<Review> Reviews => this.Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

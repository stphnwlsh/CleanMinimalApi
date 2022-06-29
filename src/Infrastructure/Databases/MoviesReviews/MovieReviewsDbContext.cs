namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews;

using System.Reflection;
using CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;
using Microsoft.EntityFrameworkCore;

internal class MovieReviewsDbContext : DbContext
{
    public MovieReviewsDbContext(DbContextOptions<MovieReviewsDbContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

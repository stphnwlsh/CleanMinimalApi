namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Models;

internal class MovieReviewsDbContext(DbContextOptions<MovieReviewsDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

/// <summary>
/// Lets the EF Core tools create the context, for example to add a migration:
/// <c>dotnet ef migrations add Name --project src/Infrastructure --output-dir Databases/MovieReviews/Migrations</c>.
/// </summary>
[ExcludeFromCodeCoverage]
internal class MovieReviewsDbContextFactory : IDesignTimeDbContextFactory<MovieReviewsDbContext>
{
    public MovieReviewsDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<MovieReviewsDbContext>()
            .UseSqlite(DependencyInjection.DefaultConnectionString)
            .Options;

        return new MovieReviewsDbContext(options);
    }
}

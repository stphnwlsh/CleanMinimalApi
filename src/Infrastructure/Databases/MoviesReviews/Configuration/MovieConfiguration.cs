namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Configuration;

using CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MovieConfiguration : EntityConfiguration<Movie>
{
    public override void Configure(EntityTypeBuilder<Movie> builder)
    {
        base.Configure(builder);

        _ = builder.HasMany(m => m.Reviews).WithOne(r => r.ReviewedMovie).HasForeignKey(r => r.ReviewedMovieId);
    }
}

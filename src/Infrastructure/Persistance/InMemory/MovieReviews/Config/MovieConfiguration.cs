namespace CleanMinimalApi.Infrastructure.Persistance.InMemory.MovieReviews.Config;

using CleanMinimalApi.Application.Entities;
using CleanMinimalApi.Infrastructure.Persistance.InMemory.Common.Config;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MovieConfiguration : EntityConfiguration<Movie>
{
    public override void Configure(EntityTypeBuilder<Movie> builder)
    {
        base.Configure(builder);

        _ = builder.HasMany(m => m.Reviews).WithOne(r => r.ReviewedMovie).HasForeignKey(r => r.ReviewedMovieId);
    }
}

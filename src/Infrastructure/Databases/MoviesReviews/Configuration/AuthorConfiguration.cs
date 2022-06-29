namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Configuration;

using CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AuthorConfiguration : EntityConfiguration<Author>
{
    public override void Configure(EntityTypeBuilder<Author> builder)
    {
        base.Configure(builder);

        _ = builder.HasMany(m => m.Reviews).WithOne(r => r.ReviewAuthor).HasForeignKey(r => r.ReviewAuthorId);
    }
}

namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

internal class AuthorConfiguration : EntityConfiguration<Author>
{
    public override void Configure(EntityTypeBuilder<Author> builder)
    {
        base.Configure(builder);

        _ = builder.HasMany(m => m.Reviews).WithOne(r => r.ReviewAuthor).HasForeignKey(r => r.ReviewAuthorId);
    }
}

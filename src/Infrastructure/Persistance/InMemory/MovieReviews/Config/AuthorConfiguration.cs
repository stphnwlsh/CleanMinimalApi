namespace CleanMinimalApi.Infrastructure.Persistance.InMemory.MovieReviews.Config;

using CleanMinimalApi.Application.Entities;
using CleanMinimalApi.Infrastructure.Persistance.InMemory.Common.Config;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AuthorConfiguration : EntityConfiguration<Author>
{
    public override void Configure(EntityTypeBuilder<Author> builder)
    {
        base.Configure(builder);

        _ = builder.HasMany(m => m.Reviews).WithOne(r => r.ReviewAuthor).HasForeignKey(r => r.ReviewAuthorId);
    }
}

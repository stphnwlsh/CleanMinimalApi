namespace CleanMinimalApi.Infrastructure.Persistance.InMemory.Common.Config;

using CleanMinimalApi.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(m => m.Id).ValueGeneratedOnAdd().IsRequired();
        _ = builder.Property(m => m.DateCreated).ValueGeneratedOnAdd().IsRequired();
        _ = builder.Property(m => m.DateModified).ValueGeneratedOnAdd().IsRequired();
    }
}

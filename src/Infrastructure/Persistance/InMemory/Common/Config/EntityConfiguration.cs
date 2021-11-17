namespace CleanMinimalApi.Infrastructure.Persistance.InMemory.Common.Config;

using CleanMinimalApi.Domain.Common.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(m => m.Id).ValueGeneratedOnAdd().IsRequired();
    }
}

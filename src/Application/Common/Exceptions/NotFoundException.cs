namespace CleanMinimalApi.Application.Common.Exceptions;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using CleanMinimalApi.Application.Common.Enums;
using CleanMinimalApi.Domain.Common.Entity;

[Serializable]
[ExcludeFromCodeCoverage]
public class NotFoundException : Exception
{
    public EntityType Entity { get; init; }

    public NotFoundException(EntityType entity)
        : base($"The {entity} with the supplied id was not found.")
    {
        this.Entity = entity;
    }

    public NotFoundException(EntityType entity, string message)
        : base(message)
    {
        this.Entity = entity;
    }

    public NotFoundException(EntityType entity, string message, Exception innerException)
        : base(message, innerException)
    {
        this.Entity = entity;
    }

    protected NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>Throws an <see cref="NotFoundException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="entityType">The entity type of the <paramref name="argument"/> parameter.</param>
    public static void ThrowIfNull(Entity? argument, EntityType entityType)
    {
        if (argument is null)
        {
            Throw(entityType);
        }
    }

    /// <summary>Throws an <see cref="NotFoundException"/></summary>
    /// <param name="entityType">The entity type of the <paramref name="argument"/> parameter.</param>
    public static void Throw(EntityType entityType)
    {
        throw new NotFoundException(entityType);
    }
}

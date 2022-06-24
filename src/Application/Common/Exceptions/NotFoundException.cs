namespace CleanMinimalApi.Application.Common.Exceptions;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Enums;

[Serializable]
[ExcludeFromCodeCoverage]
public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }

    protected NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>Throws an <see cref="NotFoundException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="entityType">The entity type of the <paramref name="argument"/> parameter.</param>
    public static void ThrowIfNull(object argument, EntityType entityType)
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
        throw new NotFoundException($"The {entityType} with the supplied id was not found.");
    }
}

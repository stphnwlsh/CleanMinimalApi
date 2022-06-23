namespace CleanMinimalApi.Application.Common.Exceptions;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Enums;

[Serializable]
[ExcludeFromCodeCoverage]
public class OrphanedReviewsException : Exception
{
    public OrphanedReviewsException(string message)
        : base(message)
    {
    }

    protected OrphanedReviewsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>Throws an <see cref="OrphanedReviewsException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="entityType">The entity type of the <paramref name="argument"/> parameter.</param>
    public static void ThrowIfNull(object argument, EntityType entityType)
    {
        if (argument is null)
        {
            Throw(entityType);
        }
    }

    /// <summary>Throws an <see cref="OrphanedReviewsException"/></summary>
    /// <param name="entityType">The entity type of the <paramref name="argument"/> parameter.</param>
    public static void Throw(EntityType entityType) => throw new NotFoundException($"The {entityType} has reviews that will be orphaned if deleted.");
}

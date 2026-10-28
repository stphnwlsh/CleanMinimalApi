namespace CleanMinimalApi.Application.Common.Exceptions;

using System;
using System.Diagnostics.CodeAnalysis;
using Enums;

public class NotFoundException(string message) : Exception(message)
{
    /// <summary>Throws a <see cref="NotFoundException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="entityType">The entity type of the <paramref name="argument"/> parameter.</param>
    public static void ThrowIfNull([NotNull] object? argument, EntityType entityType)
    {
        if (argument is null)
        {
            Throw(entityType);
        }
    }

    /// <summary>Throws a <see cref="NotFoundException"/></summary>
    /// <param name="entityType">The entity type that was not found.</param>
    [DoesNotReturn]
    public static void Throw(EntityType entityType)
    {
        throw new NotFoundException($"The {entityType} with the supplied id was not found.");
    }
}

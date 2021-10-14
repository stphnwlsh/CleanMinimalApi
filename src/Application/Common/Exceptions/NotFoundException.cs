namespace CleanMinimalApi.Application.Common.Exceptions;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using CleanMinimalApi.Application.Common.Enums;

[Serializable]
[ExcludeFromCodeCoverage]
public class NotFoundException : Exception
{
    public EntityType Entity { get; init; }

    public NotFoundException(EntityType entity)
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
}

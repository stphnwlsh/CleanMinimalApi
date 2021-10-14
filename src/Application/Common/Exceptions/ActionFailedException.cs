namespace CleanMinimalApi.Application.Common.Exceptions;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using CleanMinimalApi.Application.Common.Enums;

[Serializable]
[ExcludeFromCodeCoverage]
public class ActionFailedException : Exception
{
    public EntityType Entity { get; init; }
    public ActionType Action { get; init; }

    public ActionFailedException(EntityType entity, ActionType action)
    {
        this.Entity = entity;
        this.Action = action;
    }

    public ActionFailedException(EntityType entity, ActionType action, string message)
        : base(message)
    {
        this.Entity = entity;
        this.Action = action;
    }

    public ActionFailedException(EntityType entity, ActionType action, string message, Exception innerException)
        : base(message, innerException)
    {
        this.Entity = entity;
        this.Action = action;
    }

    protected ActionFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}

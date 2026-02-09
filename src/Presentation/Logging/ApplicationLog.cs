namespace CleanMinimalApi.Presentation.Logging;

/// <summary>
/// High-performance structured logging using LoggerMessage source generators.
/// This class provides compile-time optimized logging methods.
/// </summary>
public static partial class ApplicationLog
{
    // Endpoint logging
    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Information,
        Message = "Processing {EndpointName} request")]
    public static partial void ProcessingRequest(ILogger logger, string endpointName);

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Completed {EndpointName} request in {ElapsedMs}ms")]
    public static partial void CompletedRequest(ILogger logger, string endpointName, long elapsedMs);

    // Entity operations
    [LoggerMessage(
        EventId = 2000,
        Level = LogLevel.Information,
        Message = "Creating {EntityType} with Id {EntityId}")]
    public static partial void CreatingEntity(ILogger logger, string entityType, Guid entityId);

    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Information,
        Message = "Updated {EntityType} with Id {EntityId}")]
    public static partial void UpdatedEntity(ILogger logger, string entityType, Guid entityId);

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Information,
        Message = "Deleted {EntityType} with Id {EntityId}")]
    public static partial void DeletedEntity(ILogger logger, string entityType, Guid entityId);

    [LoggerMessage(
        EventId = 2003,
        Level = LogLevel.Warning,
        Message = "{EntityType} with Id {EntityId} not found")]
    public static partial void EntityNotFound(ILogger logger, string entityType, Guid entityId);

    // Error logging
    [LoggerMessage(
        EventId = 3000,
        Level = LogLevel.Error,
        Message = "Error processing {Operation}: {ErrorMessage}")]
    public static partial void ErrorProcessingOperation(ILogger logger, Exception exception, string operation, string errorMessage);

    [LoggerMessage(
        EventId = 3001,
        Level = LogLevel.Warning,
        Message = "Validation failed for {EndpointName}: {ValidationErrors}")]
    public static partial void ValidationFailed(ILogger logger, string endpointName, string validationErrors);

    // Database operations
    [LoggerMessage(
        EventId = 4000,
        Level = LogLevel.Debug,
        Message = "Executing database query: {QueryType}")]
    public static partial void ExecutingQuery(ILogger logger, string queryType);

    [LoggerMessage(
        EventId = 4001,
        Level = LogLevel.Debug,
        Message = "Database query completed: {QueryType} returned {ResultCount} results")]
    public static partial void QueryCompleted(ILogger logger, string queryType, int resultCount);
}

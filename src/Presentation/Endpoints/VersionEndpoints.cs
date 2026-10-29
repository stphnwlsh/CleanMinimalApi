namespace CleanMinimalApi.Presentation.Endpoints;

using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Entities = Application.Versions.Entities;
using Queries = Application.Versions.Queries;

public static class VersionEndpoints
{
    public static WebApplication MapVersionEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/version")
            .WithTags("Version")
            .AllowAnonymous();

        _ = root.MapGet("/", GetVersion)
            .WithName(nameof(GetVersion))
            .Produces<Entities.Version>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    /// <summary>
    /// Look up the application version details
    /// </summary>
    /// <remarks>
    /// Returns the file and informational versions of the running application.
    ///
    /// Sample request:
    ///
    ///     GET /api/version
    /// </remarks>
    /// <response code="200">The current application version details.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Entities.Version>> GetVersion([FromServices] ISender sender, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetVersion.GetVersionQuery(), cancellationToken));
    }
}

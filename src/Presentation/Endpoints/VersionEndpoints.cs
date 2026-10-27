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
            .WithTags("version");

        _ = root.MapGet("/", GetVersion)
            .Produces<Entities.Version>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup the application version details")
            .WithDescription("\n    GET /Version");

        return app;
    }

    public static async Task<Ok<Entities.Version>> GetVersion([FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetVersion.GetVersionQuery()));
    }
}

namespace CleanMinimalApi.Presentation.Endpoints;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Entities = Application.Versions.Entities;
using Queries = Application.Versions.Queries;

public static class VersionEndpoints
{
    public static WebApplication MapVersionEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/version")
            .WithTags("version")
            .WithOpenApi();

        _ = root.MapGet("/", GetVersion)
            .Produces<List<Entities.Version>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup the application version details")
            .WithDescription("\n    GET /Version");

        return app;
    }

    public static async Task<IResult> GetVersion(IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new Queries.GetVersion.GetVersionQuery()));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

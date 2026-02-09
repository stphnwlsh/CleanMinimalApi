namespace CleanMinimalApi.Presentation.Endpoints;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

    public static async Task<Results<Ok<Entities.Version>, ProblemHttpResult>> GetVersion(IMediator mediator)
    {
        try
        {
            return TypedResults.Ok(await mediator.Send(new Queries.GetVersion.GetVersionQuery()));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

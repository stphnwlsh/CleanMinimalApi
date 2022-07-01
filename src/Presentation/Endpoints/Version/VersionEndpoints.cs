namespace CleanMinimalApi.Presentation.Endpoints.Version;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Errors;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Entities = Application.Versions.Entities;
using Queries = Application.Versions.Queries;

[ExcludeFromCodeCoverage]
public static class VersionEndpoints
{
    public static WebApplication MapVersionEndpoints(this WebApplication app)
    {
        _ = app.MapGet("/api/version",
                async (IMediator mediator) =>
                    Results.Ok(await mediator.Send(new Queries.GetVersion.GetVersionQuery())))
            .WithTags("Version")
            .WithMetadata(new SwaggerOperationAttribute("Lookup the application version details", "\n    GET /Version"))
            .Produces<List<Entities.Version>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        return app;
    }
}

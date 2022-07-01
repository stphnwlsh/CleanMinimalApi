namespace CleanMinimalApi.Presentation.Endpoints.Authors;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Errors;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Entities = Application.Authors.Entities;
using Queries = Application.Authors.Queries;

[ExcludeFromCodeCoverage]
public static class AuthorsEndpoints
{
    public static WebApplication MapAuthorEndpoints(this WebApplication app)
    {
        _ = app.MapGet("/api/authors",
                async (IMediator mediator) =>
                    Results.Ok(await mediator.Send(new Queries.GetAuthors.GetAuthorsQuery())))
            .WithTags("Authors")
            .WithMetadata(new SwaggerOperationAttribute("Lookup all Authors", "\n    GET /Authors"))
            .Produces<List<Entities.Author>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        _ = app.MapGet(
                "/api/authors/{id:guid}",
                async (IMediator mediator, Guid id) =>
                    Results.Ok(await mediator.Send(new Queries.GetAuthorById.GetAuthorByIdQuery { Id = id })))
            .WithTags("Authors")
            .WithMetadata(new SwaggerOperationAttribute("Lookup an Author by their Id", "\n    GET /Authors/00000000-0000-0000-0000-000000000000"))
            .Produces<Entities.Author>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        return app;
    }
}

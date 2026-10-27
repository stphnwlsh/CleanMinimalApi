namespace CleanMinimalApi.Presentation.Endpoints;

using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Entities = Application.Authors.Entities;
using Queries = Application.Authors.Queries;

public static class AuthorEndpoints
{
    public static WebApplication MapAuthorEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/author")
            .WithTags("author")
            .WithDescription("Lookup and Find Authors");

        _ = root.MapGet("/", GetAuthors)
            .Produces<List<Entities.Author>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Authors")
            .WithDescription("\n    GET /author");

        _ = root.MapGet("/{id:guid}", GetAuthorById)
            .Produces<Entities.Author>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup an Author by their Id")
            .WithDescription("\n    GET /author/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<Ok<List<Entities.Author>>> GetAuthors([FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetAuthors.GetAuthorsQuery()));
    }

    public static async Task<Ok<Entities.Author>> GetAuthorById([FromRoute] Guid id, [FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetAuthorById.GetAuthorByIdQuery
        {
            Id = id
        }));
    }
}

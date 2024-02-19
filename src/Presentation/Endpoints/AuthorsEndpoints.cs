namespace CleanMinimalApi.Presentation.Endpoints;

using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities = Application.Authors.Entities;
using Queries = Application.Authors.Queries;

public static class AuthorsEndpoints
{
    public static WebApplication MapAuthorEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/author")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("author")
            .WithDescription("Lookup and Find Authors")
            .WithOpenApi();

        _ = root.MapGet("/", GetAuthors)
            .Produces<List<Entities.Author>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Authors")
            .WithDescription("\n    GET /author");

        _ = root.MapGet("/{id}", GetAuthorById)
            .Produces<Entities.Author>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup an Author by their Id")
            .WithDescription("\n    GET /author/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetAuthors([FromServices] IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new Queries.GetAuthors.GetAuthorsQuery()));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetAuthorById([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new Queries.GetAuthorById.GetAuthorByIdQuery
            {
                Id = id
            }));
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

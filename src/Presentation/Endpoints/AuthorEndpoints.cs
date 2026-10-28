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
            .WithTags("Authors");

        _ = root.MapGet("/", GetAuthors)
            .WithName(nameof(GetAuthors))
            .Produces<List<Entities.Author>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        _ = root.MapGet("/{id:guid}", GetAuthorById)
            .WithName(nameof(GetAuthorById))
            .Produces<Entities.Author>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    /// <summary>
    /// Lookup all Authors
    /// </summary>
    /// <remarks>
    /// Returns every author along with the reviews they have written.
    ///
    /// Sample request:
    ///
    ///     GET /api/author
    /// </remarks>
    /// <response code="200">A list of all the Authors.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<List<Entities.Author>>> GetAuthors([FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetAuthors.GetAuthorsQuery()));
    }

    /// <summary>
    /// Lookup an Author by their Id
    /// </summary>
    /// <remarks>
    /// Returns a single author along with the reviews they have written.
    ///
    /// Sample request:
    ///
    ///     GET /api/author/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </remarks>
    /// <param name="id">The unique identifier of the author.</param>
    /// <response code="200">The Author with the specified Id.</response>
    /// <response code="400">The Id is not valid.</response>
    /// <response code="404">No Author exists with the specified Id.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Entities.Author>> GetAuthorById([FromRoute] Guid id, [FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetAuthorById.GetAuthorByIdQuery
        {
            Id = id
        }));
    }
}

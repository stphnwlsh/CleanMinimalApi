namespace CleanMinimalApi.Presentation.Endpoints;

using Application.Common.Paging;
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
        var root = app.MapGroup("/api/authors")
            .WithTags("Authors")
            .RequireAuthorization();

        _ = root.MapGet("/", GetAuthors)
            .WithName(nameof(GetAuthors))
            .Produces<Page<Entities.Author>>()
            .ProducesValidationProblem()
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
    /// List Authors
    /// </summary>
    /// <remarks>
    /// Returns one page of authors, ordered by last name then first name, along with the reviews they have
    /// written. Add a search term to return only the authors whose first, last or full name contains it,
    /// ignoring case. To fetch the next page, add the limit to the offset.
    ///
    /// Sample request:
    ///
    ///     GET /api/authors?query=smith&amp;offset=0&amp;limit=10
    /// </remarks>
    /// <param name="query">The words to look for in the name, ignoring case. Leave it out to list every author.</param>
    /// <param name="offset">The number of authors to skip, 0 for the first page.</param>
    /// <param name="limit">The largest number of authors to return, from 1 to 50.</param>
    /// <response code="200">One page of Authors.</response>
    /// <response code="400">The search term, offset or limit is not valid.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Page<Entities.Author>>> GetAuthors(
        [FromQuery] string? query,
        [FromServices] ISender sender,
        CancellationToken cancellationToken,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = Paging.DefaultLimit)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetAuthors.GetAuthorsQuery
        {
            Query = query,
            Offset = offset,
            Limit = limit
        }, cancellationToken));
    }

    /// <summary>
    /// Look up an Author by their Id
    /// </summary>
    /// <remarks>
    /// Returns a single author along with the reviews they have written.
    ///
    /// Sample request:
    ///
    ///     GET /api/authors/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </remarks>
    /// <param name="id">The unique identifier of the author.</param>
    /// <response code="200">The Author with the specified Id.</response>
    /// <response code="400">The Id is not valid.</response>
    /// <response code="404">No Author exists with the specified Id.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Entities.Author>> GetAuthorById(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetAuthorById.GetAuthorByIdQuery
        {
            Id = id
        }, cancellationToken));
    }
}

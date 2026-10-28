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
        var root = app.MapGroup("/api/author")
            .WithTags("Authors");

        _ = root.MapGet("/", GetAuthors)
            .WithName(nameof(GetAuthors))
            .Produces<List<Entities.Author>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        _ = root.MapGet("/search", SearchAuthors)
            .WithName(nameof(SearchAuthors))
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
    /// Search Authors by name
    /// </summary>
    /// <remarks>
    /// Returns one page of authors whose first name, last name or full name contains the search term,
    /// ignoring case, ordered by last name then first name. Leave the search term out to page through
    /// every author. When more results remain, the response holds a nextCursor. Send it back as the
    /// cursor to fetch the next page.
    ///
    /// Sample request:
    ///
    ///     GET /api/author/search?query=smith&amp;limit=10
    /// </remarks>
    /// <param name="query">The words to look for in the name, ignoring case. Leave it out to list every author.</param>
    /// <param name="limit">The largest number of authors to return, from 1 to 50.</param>
    /// <param name="cursor">The nextCursor from a previous page. Leave it out for the first page.</param>
    /// <response code="200">One page of matching Authors.</response>
    /// <response code="400">The search term, limit or cursor is not valid.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Page<Entities.Author>>> SearchAuthors(
        [FromQuery] string query,
        [FromServices] ISender sender,
        [FromQuery] int limit = Paging.DefaultLimit,
        [FromQuery] string cursor = null)
    {
        return TypedResults.Ok(await sender.Send(new Queries.SearchAuthors.SearchAuthorsQuery
        {
            Query = query,
            Limit = limit,
            Cursor = cursor
        }));
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

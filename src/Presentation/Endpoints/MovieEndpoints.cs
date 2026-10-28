namespace CleanMinimalApi.Presentation.Endpoints;

using Application.Common.Paging;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Entities = Application.Movies.Entities;
using Queries = Application.Movies.Queries;

public static class MovieEndpoints
{
    public static WebApplication MapMovieEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/movie")
            .WithTags("Movies");

        _ = root.MapGet("/", GetMovies)
            .WithName(nameof(GetMovies))
            .Produces<List<Entities.Movie>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        _ = root.MapGet("/search", SearchMovies)
            .WithName(nameof(SearchMovies))
            .Produces<Page<Entities.Movie>>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        _ = root.MapGet("/{id:guid}", GetMovieById)
            .WithName(nameof(GetMovieById))
            .Produces<Entities.Movie>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    /// <summary>
    /// Lookup all Movies
    /// </summary>
    /// <remarks>
    /// Returns every movie along with the reviews written about it.
    ///
    /// Sample request:
    ///
    ///     GET /api/movie
    /// </remarks>
    /// <response code="200">A list of all the Movies.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<List<Entities.Movie>>> GetMovies([FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetMovies.GetMoviesQuery()));
    }

    /// <summary>
    /// Search Movies by title
    /// </summary>
    /// <remarks>
    /// Returns one page of movies whose title contains the search term, ignoring case, ordered by title.
    /// Leave the search term out to page through every movie. When more results remain, the response
    /// holds a nextCursor. Send it back as the cursor to fetch the next page.
    ///
    /// Sample request:
    ///
    ///     GET /api/movie/search?query=the&amp;limit=10
    /// </remarks>
    /// <param name="query">The words to look for in the title, ignoring case. Leave it out to list every movie.</param>
    /// <param name="limit">The largest number of movies to return, from 1 to 50.</param>
    /// <param name="cursor">The nextCursor from a previous page. Leave it out for the first page.</param>
    /// <response code="200">One page of matching Movies.</response>
    /// <response code="400">The search term, limit or cursor is not valid.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Page<Entities.Movie>>> SearchMovies(
        [FromQuery] string query,
        [FromServices] ISender sender,
        [FromQuery] int limit = Paging.DefaultLimit,
        [FromQuery] string cursor = null)
    {
        return TypedResults.Ok(await sender.Send(new Queries.SearchMovies.SearchMoviesQuery
        {
            Query = query,
            Limit = limit,
            Cursor = cursor
        }));
    }

    /// <summary>
    /// Lookup a Movie by its Id
    /// </summary>
    /// <remarks>
    /// Returns a single movie along with the reviews written about it.
    ///
    /// Sample request:
    ///
    ///     GET /api/movie/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </remarks>
    /// <param name="id">The unique identifier of the movie.</param>
    /// <response code="200">The Movie with the specified Id.</response>
    /// <response code="400">The Id is not valid.</response>
    /// <response code="404">No Movie exists with the specified Id.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Entities.Movie>> GetMovieById([FromRoute] Guid id, [FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetMovieById.GetMovieByIdQuery
        {
            Id = id
        }));
    }
}

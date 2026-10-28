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
        var root = app.MapGroup("/api/movies")
            .WithTags("Movies")
            .RequireAuthorization();

        _ = root.MapGet("/", GetMovies)
            .WithName(nameof(GetMovies))
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
    /// List Movies
    /// </summary>
    /// <remarks>
    /// Returns one page of movies, ordered by title, along with the reviews written about them. Add a search
    /// term to return only the movies whose title contains it, ignoring case. To fetch the next page, add the
    /// limit to the offset.
    ///
    /// Sample request:
    ///
    ///     GET /api/movies?query=the&amp;offset=0&amp;limit=10
    /// </remarks>
    /// <param name="query">The words to look for in the title, ignoring case. Leave it out to list every movie.</param>
    /// <param name="offset">The number of movies to skip, 0 for the first page.</param>
    /// <param name="limit">The largest number of movies to return, from 1 to 50.</param>
    /// <response code="200">One page of Movies.</response>
    /// <response code="400">The search term, offset or limit is not valid.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Page<Entities.Movie>>> GetMovies(
        [FromQuery] string? query,
        [FromServices] ISender sender,
        CancellationToken cancellationToken,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = Paging.DefaultLimit)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetMovies.GetMoviesQuery
        {
            Query = query,
            Offset = offset,
            Limit = limit
        }, cancellationToken));
    }

    /// <summary>
    /// Look up a Movie by its Id
    /// </summary>
    /// <remarks>
    /// Returns a single movie along with the reviews written about it.
    ///
    /// Sample request:
    ///
    ///     GET /api/movies/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </remarks>
    /// <param name="id">The unique identifier of the movie.</param>
    /// <response code="200">The Movie with the specified Id.</response>
    /// <response code="400">The Id is not valid.</response>
    /// <response code="404">No Movie exists with the specified Id.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Entities.Movie>> GetMovieById(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetMovieById.GetMovieByIdQuery
        {
            Id = id
        }, cancellationToken));
    }
}

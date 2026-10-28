namespace CleanMinimalApi.Presentation.Endpoints;

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

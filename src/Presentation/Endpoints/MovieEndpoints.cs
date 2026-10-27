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
            .WithTags("movie")
            .WithDescription("Lookup and Find Movies");

        _ = root.MapGet("/", GetMovies)
            .Produces<List<Entities.Movie>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Movies")
            .WithDescription("\n    GET /movie");

        _ = root.MapGet("/{id:guid}", GetMovieById)
            .Produces<Entities.Movie>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Movie by its Id")
            .WithDescription("\n    GET /movie/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<Ok<List<Entities.Movie>>> GetMovies([FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetMovies.GetMoviesQuery()));
    }

    public static async Task<Ok<Entities.Movie>> GetMovieById([FromRoute] Guid id, [FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetMovieById.GetMovieByIdQuery
        {
            Id = id
        }));
    }
}

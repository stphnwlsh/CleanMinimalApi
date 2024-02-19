namespace CleanMinimalApi.Presentation.Endpoints;

using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities = Application.Movies.Entities;
using Queries = Application.Movies.Queries;

public static class MoviesEndpoints
{
    public static WebApplication MapMovieEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/movie")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("movie")
            .WithDescription("Lookup and Find Movies")
            .WithOpenApi();

        _ = root.MapGet("/", GetMovies)
            .Produces<List<Entities.Movie>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Movies")
            .WithDescription("\n    GET /movie");

        _ = root.MapGet("/{id}", GetMovieById)
            .Produces<Entities.Movie>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Movie by its Id")
            .WithDescription("\n    GET /movie/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetMovies([FromServices] IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new Queries.GetMovies.GetMoviesQuery()));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetMovieById([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new Queries.GetMovieById.GetMovieByIdQuery
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

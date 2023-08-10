namespace CleanMinimalApi.Presentation.Endpoints;

using System.Diagnostics.CodeAnalysis;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Entities = Application.Movies.Entities;
using Queries = Application.Movies.Queries;

[ExcludeFromCodeCoverage]
public static class MoviesEndpoints
{
    public static WebApplication MapMovieEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/movies")
            .WithTags("movies")
            .WithOpenApi();

        _ = root.MapGet("/", GetAllMovies)
            .Produces<List<Entities.Movie>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Movies")
            .WithDescription("\n    GET /movies");

        _ = root.MapGet("/{id:guid}", GetMovieById)
            .AddEndpointFilter<ValidationFilter<Guid>>()
            .Produces<Entities.Movie>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithSummary("Lookup a Movies by its Ids")
            .WithDescription("\n    GET /movies/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetAllMovies(IMediator mediator)
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

    public static async Task<IResult> GetMovieById(Guid id, IMediator mediator)
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

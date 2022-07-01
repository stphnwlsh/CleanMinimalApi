namespace CleanMinimalApi.Presentation.Endpoints.Movies;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Errors;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Entities = Application.Movies.Entities;
using Queries = Application.Movies.Queries;

[ExcludeFromCodeCoverage]
public static class MoviesEndpoints
{
    public static WebApplication MapMovieEndpoints(this WebApplication app)
    {
        _ = app.MapGet("/api/movies",
                async (IMediator mediator) =>
                    Results.Ok(await mediator.Send(new Queries.GetMovies.GetMoviesQuery())))
            .WithTags("Movies")
            .WithMetadata(new SwaggerOperationAttribute("Lookup all Movies", "\n    GET /Movies"))
            .Produces<List<Entities.Movie>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        _ = app.MapGet(
                "/api/movies/{id:guid}",
                async (IMediator mediator, Guid id) =>
                    Results.Ok(await mediator.Send(new Queries.GetMovieById.GetMovieByIdQuery { Id = id })))
            .WithTags("Movies")
            .WithMetadata(new SwaggerOperationAttribute("Lookup a Movie by its Id", "\n    GET /Movies/00000000-0000-0000-0000-000000000000"))
            .Produces<Entities.Movie>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        return app;
    }
}

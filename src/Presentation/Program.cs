using System.Net.Mime;
using CleanMinimalApi.Presentation.Errors;
using CleanMinimalApi.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Authors = CleanMinimalApi.Application.Authors;
using Movies = CleanMinimalApi.Application.Movies;
using Reviews = CleanMinimalApi.Application.Reviews;
using Versions = CleanMinimalApi.Application.Versions;

var builder = WebApplication
                .CreateBuilder(args)
                .ConfigureBuilder();
var app = builder
            .Build()
            .ConfigureApplication();

#region Versions

_ = app.MapGet("/version", async (IMediator mediator) => Results.Ok(await mediator.Send(new Versions.Queries.GetVersion.GetVersionQuery())))
    .WithGroupName("Version")
    .Produces<Version>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

#endregion Versions

#region Authors

_ = app.MapGet("/authors", async ([FromServices] IMediator mediator) => Results.Ok(await mediator.Send(new Authors.Queries.GetAuthors.GetAuthorsQuery())))
    .WithGroupName("Authors")
    .Produces<List<Authors.Entities.Author>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapGet("/authors/{id:guid}", async ([FromServices] IMediator mediator, Guid id) => Results.Ok(await mediator.Send(new Authors.Queries.GetAuthorById.GetAuthorByIdQuery { Id = id })))
    .WithGroupName("Authors")
    .Produces<Authors.Entities.Author>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

#endregion Authors

#region Movies

_ = app.MapGet("/movies", async (IMediator mediator) => Results.Ok(await mediator.Send(new Movies.GetMovies.GetMoviesQuery())))
    .WithGroupName("Movies")
    .Produces<List<Movies.Entities.Movie>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapGet("/movies/{id}", async (IMediator mediator, Guid id) => Results.Ok(await mediator.Send(new Movies.GetMovieById.GetMovieByIdQuery { Id = id })))
    .WithGroupName("Movies")
    .Produces<Movies.Entities.Movie>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

#endregion Movies

#region Reviews

_ = app.MapPost("/reviews", async (IMediator mediator, HttpRequest httpRequest, Reviews.Commands.CreateReview.CreateReviewCommand command) => Results.Created(UriHelper.GetEncodedUrl(httpRequest), await mediator.Send(command)))
    .WithGroupName("Reviews")
    .Produces<Reviews.Entities.Review>(StatusCodes.Status201Created, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapDelete("/reviews/{id}", async (IMediator mediator, Guid id) =>
    {
        _ = await mediator.Send(new Reviews.Commands.DeleteReview.DeleteReviewCommand { Id = id });

        return Results.NoContent();
    })
    .WithGroupName("Reviews")
    .Produces(StatusCodes.Status204NoContent)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapGet("/reviews", async (IMediator mediator) => Results.Ok(await mediator.Send(new Reviews.Queries.GetReviews.GetReviewsQuery())))
    .WithGroupName("Reviews")
    .Produces<List<Reviews.Entities.Review>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapGet("/reviews/{id}", async (IMediator mediator, Guid id) => Results.Ok(await mediator.Send(new Reviews.Queries.GetReviewById.GetReviewByIdQuery { Id = id })))
    .WithGroupName("Reviews")
    .Produces<Reviews.Entities.Review>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapPut("/reviews/{id}", async (IMediator mediator, Guid id, Reviews.Commands.UpdateReview.UpdateReviewCommand command) =>
    {
        command.Id = id;

        _ = await mediator.Send(command);

        return Results.NoContent();
    })
    .WithGroupName("Reviews")
    .Produces(StatusCodes.Status204NoContent)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

#endregion Reviews

try
{
    Log.Information("Starting host");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

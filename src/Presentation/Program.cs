using System.Net.Mime;
using CleanMinimalApi.Application.Reviews.Create;
using CleanMinimalApi.Application.Reviews.Delete;
using CleanMinimalApi.Application.Versions.ReadVersion;
using CleanMinimalApi.Domain.Authors.Entities;
using CleanMinimalApi.Domain.Movies.Entities;
using CleanMinimalApi.Domain.Reviews.Entities;
using CleanMinimalApi.Domain.Version;
using CleanMinimalApi.Presentation.Errors;
using CleanMinimalApi.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using Authors = CleanMinimalApi.Application.Authors;
using Movies = CleanMinimalApi.Application.Movies;
using Reviews = CleanMinimalApi.Application.Reviews;

var app = WebApplication.CreateBuilder(args).ConfigureBuilder().Build().ConfigureApp();

#region Versions

_ = app.MapGet("/version", async (IMediator mediator) => Results.Ok(await mediator.Send(new ReadVersionQuery())))
    .WithGroupName("Version")
    .Produces<ApplicationVersion>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

#endregion Versions

#region Authors

_ = app.MapGet("/authors", async (IMediator mediator) => Results.Ok(await mediator.Send(new Authors.ReadAll.ReadAllQuery())))
    .WithGroupName("Authors")
    .Produces<List<Author>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapGet("/authors/{id}", async (IMediator mediator, Guid id) => Results.Ok(await mediator.Send(new Authors.ReadById.ReadByIdQuery { Id = id })))
    .WithGroupName("Authors")
    .Produces<Author>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

#endregion Authors

#region Movies

_ = app.MapGet("/movies", async (IMediator mediator) => Results.Ok(await mediator.Send(new Movies.ReadAll.ReadAllQuery())))
    .WithGroupName("Movies")
    .Produces<List<Movie>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapGet("/movies/{id}", async (IMediator mediator, Guid id) => Results.Ok(await mediator.Send(new Movies.ReadById.ReadByIdQuery { Id = id })))
    .WithGroupName("Movies")
    .Produces<Movie>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

#endregion Movies


#region Reviews

_ = app.MapPost("/reviews", async (IMediator mediator, HttpRequest httpRequest, CreateCommand command) => Results.Created(UriHelper.GetEncodedUrl(httpRequest), await mediator.Send(command)))
    .WithGroupName("Reviews")
    .Produces<Review>(StatusCodes.Status201Created, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapDelete("/reviews/{id}", async (IMediator mediator, Guid id) =>
    {
        _ = await mediator.Send(new DeleteCommand { Id = id });

        return Results.NoContent();
    })
    .WithGroupName("Notes")
    .Produces(StatusCodes.Status204NoContent)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapGet("/reviews", async (IMediator mediator) => Results.Ok(await mediator.Send(new Reviews.ReadAll.ReadAllQuery())))
    .WithGroupName("Reviews")
    .Produces<List<Review>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapGet("/reviews/{id}", async (IMediator mediator, Guid id) => Results.Ok(await mediator.Send(new Reviews.ReadById.ReadByIdQuery { Id = id })))
    .WithGroupName("Reviews")
    .Produces<Review>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

#endregion Reviews

// #region Notes

// _ = app.MapPut("/notes/{id}", async (IMediator mediator, int id, UpdateNoteRequest request) =>
//     {
//         _ = await mediator.Send(new UpdateNoteCommand(id, request.Text));

//         return Results.NoContent();
//     })
//     .WithGroupName("Notes")
//     .Produces<Note>(StatusCodes.Status204NoContent)
//     .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
//     .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
//     .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

// #endregion Notes

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

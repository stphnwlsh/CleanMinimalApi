using System.Net.Mime;
using CleanMinimalApi.Application.Versions.ApiVersion;
using CleanMinimalApi.Domain.Authors.Entities;
using CleanMinimalApi.Domain.Entities.Versions;
using CleanMinimalApi.Domain.Movies.Entities;
using CleanMinimalApi.Domain.Reviews.Entities;
using CleanMinimalApi.Presentation.Errors;
using CleanMinimalApi.Presentation.Extensions;
using MediatR;
using Serilog;
using Authors = CleanMinimalApi.Application.Authors;
using Movies = CleanMinimalApi.Application.Movies;
using Reviews = CleanMinimalApi.Application.Reviews;

var app = WebApplication.CreateBuilder(args).ConfigureBuilder().Build().ConfigureApp();

#region Versions

_ = app.MapGet("/version", async (IMediator mediator) => Results.Ok(await mediator.Send(new ApiVersionQuery())))
    .WithGroupName("Version")
    .Produces<ApiVersion>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
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

// _ = app.MapGet("/notes", async (IMediator mediator) => Results.Ok(await mediator.Send(new ListNotesQuery())))
//     .WithGroupName("Notes")
//     .Produces<List<Note>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
//     .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

// _ = app.MapGet("/notes/{id}", async (IMediator mediator, int id) => Results.Ok(await mediator.Send(new LookupNoteQuery(id))))
//     .WithGroupName("Notes")
//     .Produces<Note>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
//     .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
//     .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
//     .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

// _ = app.MapPost("/notes", async (IMediator mediator, HttpRequest httpRequest, CreateNoteRequest request) => Results.Created(UriHelper.GetEncodedUrl(httpRequest), await mediator.Send(new CreateNoteCommand(request.Text))))
//     .WithGroupName("Notes")
//     .Produces<ApiError>(StatusCodes.Status201Created, contentType: MediaTypeNames.Application.Json)
//     .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
//     .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

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

// _ = app.MapDelete("/notes/{id}", async (IMediator mediator, int id) =>
//     {
//         _ = await mediator.Send(new DeleteNoteCommand(id));

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

using System.Net.Mime;
using CleanMinimalApi.Application.Notes.Create;
using CleanMinimalApi.Application.Notes.Delete;
using CleanMinimalApi.Application.Notes.List;
using CleanMinimalApi.Application.Notes.Lookup;
using CleanMinimalApi.Application.Notes.Update;
using CleanMinimalApi.Domain.Entities.Notes;
using CleanMinimalApi.Presentation.Errors;
using CleanMinimalApi.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;

var app = WebApplication.CreateBuilder(args).ConfigureBuilder().Build().ConfigureApp();

_ = app.MapGet("/notes", async (IMediator mediator) => Results.Ok(await mediator.Send(new ListNotesQuery())))
    .WithGroupName("Notes")
    .Produces<List<Note>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapGet("/notes/{id}", async (IMediator mediator, int id) => Results.Ok(await mediator.Send(new LookupNoteQuery(id))))
    .WithGroupName("Notes")
    .Produces<Note>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapPost("/notes", async (IMediator mediator, HttpRequest httpRequest, CreateNoteRequest request) => Results.Created(UriHelper.GetEncodedUrl(httpRequest), await mediator.Send(new CreateNoteCommand(request.Text))))
    .WithGroupName("Notes")
    .Produces<ApiError>(StatusCodes.Status201Created, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapPut("/notes/{id}", async (IMediator mediator, int id, UpdateNoteRequest request) =>
    {
        _ = await mediator.Send(new UpdateNoteCommand(id, request.Text));

        return Results.NoContent();
    })
    .WithGroupName("Notes")
    .Produces<Note>(StatusCodes.Status204NoContent)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

_ = app.MapDelete("/notes/{id}", async (IMediator mediator, int id) =>
    {
        _ = await mediator.Send(new DeleteNoteCommand(id));

        return Results.NoContent();
    })
    .WithGroupName("Notes")
    .Produces<Note>(StatusCodes.Status204NoContent)
    .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
    .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

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

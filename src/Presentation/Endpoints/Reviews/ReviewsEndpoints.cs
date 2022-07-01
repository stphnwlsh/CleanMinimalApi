namespace CleanMinimalApi.Presentation.Endpoints.Reviews;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Errors;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Requests;
using Swashbuckle.AspNetCore.Annotations;
using Commands = Application.Reviews.Commands;
using Entities = Application.Reviews.Entities;
using Queries = Application.Reviews.Queries;

[ExcludeFromCodeCoverage]
public static class ReviewsEndpoints
{
    public static WebApplication MapReviewEndpoints(this WebApplication app)
    {
        _ = app.MapGet(
                "/api/reviews",
                async (IMediator mediator) =>
                    Results.Ok(await mediator.Send(new Queries.GetReviews.GetReviewsQuery())))
            .WithTags("Reviews")
            .WithMetadata(new SwaggerOperationAttribute("Lookup all Reviews", "\n    GET /Reviews"))
            .Produces<List<Entities.Review>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        _ = app.MapGet(
                "/api/reviews/{id:guid}",
                async (IMediator mediator, Guid id) =>
                    Results.Ok(await mediator.Send(new Queries.GetReviewById.GetReviewByIdQuery { Id = id })))
            .WithTags("Reviews")
            .WithMetadata(new SwaggerOperationAttribute("Lookup a Review by its Id", "\n    GET /Reviews/00000000-0000-0000-0000-000000000000"))
            .Produces<Entities.Review>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        _ = app.MapPost(
                "/api/reviews",
                async (IMediator mediator, HttpRequest httpRequest, CreateReviewRequest request) =>
                {
                    var command = new Commands.CreateReview.CreateReviewCommand()
                    {
                        AuthorId = request.AuthorId,
                        MovieId = request.MovieId,
                        Stars = request.Stars
                    };

                    return Results.Created(UriHelper.GetEncodedUrl(httpRequest), await mediator.Send(command));
                })
            .WithTags("Reviews")
            .WithMetadata(new SwaggerOperationAttribute("Create a Review", "\n    POST /Reviews\n     {         \"authorId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"movieId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"stars\": 5       }"))
            .Produces<Entities.Review>(StatusCodes.Status201Created, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        _ = app.MapDelete(
                "/api/reviews/{id:guid}",
                async (IMediator mediator, Guid id) =>
                {
                    _ = await mediator.Send(new Commands.DeleteReview.DeleteReviewCommand { Id = id });

                    return Results.NoContent();
                })
            .WithTags("Reviews")
            .WithMetadata(new SwaggerOperationAttribute("Delete a Review by its Id", "\n    DELETE /Reviews/00000000-0000-0000-0000-000000000000"))
            .Produces<Entities.Review>(StatusCodes.Status204NoContent, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        _ = app.MapPut(
                "/api/reviews/{id:guid}",
                async (IMediator mediator, Guid id, UpdateReviewRequest request) =>
                {
                    var command = new Commands.UpdateReview.UpdateReviewCommand
                    {
                        Id = id,
                        AuthorId = request.AuthorId,
                        MovieId = request.MovieId,
                        Stars = request.Stars
                    };

                    _ = await mediator.Send(command);

                    return Results.NoContent();
                })
            .WithTags("Reviews")
            .WithMetadata(new SwaggerOperationAttribute("Update a Review", "\n    PUT /Reviews/00000000-0000-0000-0000-000000000000\n     {         \"authorId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"movieId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"stars\": 5       }"))
            .Produces<Entities.Review>(StatusCodes.Status201Created, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);


        return app;
    }
}

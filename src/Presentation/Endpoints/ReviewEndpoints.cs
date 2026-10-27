namespace CleanMinimalApi.Presentation.Endpoints;

using CleanMinimalApi.Presentation.Requests;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Commands = Application.Reviews.Commands;
using Entities = Application.Reviews.Entities;
using Queries = Application.Reviews.Queries;

public static class ReviewEndpoints
{
    public static WebApplication MapReviewEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/review")
            .WithTags("review")
            .WithDescription("Lookup, Find and Manipulate Reviews");

        _ = root.MapGet("/", GetReviews)
            .Produces<List<Entities.Review>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Reviews")
            .WithDescription("\n    GET /review");

        _ = root.MapGet("/{id:guid}", GetReviewById)
            .Produces<Entities.Review>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Review by its Ids")
            .WithDescription("\n    GET /review/00000000-0000-0000-0000-000000000000");

        _ = root.MapPost("/", CreateReview)
            .Produces<Entities.Review>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a Review")
            .WithDescription("\n    POST /review\n     {         \"authorId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"movieId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"stars\": 5       }");

        _ = root.MapDelete("/{id:guid}", DeleteReview)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Review by its Id")
            .WithDescription("\n    DELETE /review/00000000-0000-0000-0000-000000000000");

        _ = root.MapPut("/{id:guid}", UpdateReview)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a Review")
            .WithDescription("\n    PUT /review/00000000-0000-0000-0000-000000000000\n     {         \"authorId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"movieId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"stars\": 5       }");

        return app;
    }

    public static async Task<Ok<List<Entities.Review>>> GetReviews([FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetReviews.GetReviewsQuery()));
    }

    public static async Task<Ok<Entities.Review>> GetReviewById([FromRoute] Guid id, [FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetReviewById.GetReviewByIdQuery
        {
            Id = id
        }));
    }

    public static async Task<Created<Entities.Review>> CreateReview([FromBody] CreateReviewRequest request, [FromServices] ISender sender)
    {
        var response = await sender.Send(new Commands.CreateReview.CreateReviewCommand
        {
            AuthorId = request.AuthorId,
            MovieId = request.MovieId,
            Stars = request.Stars
        });

        return TypedResults.Created($"/api/review/{response.Id}", response);
    }

    public static async Task<NoContent> UpdateReview([FromRoute] Guid id, [FromBody] UpdateReviewRequest request, [FromServices] ISender sender)
    {
        _ = await sender.Send(new Commands.UpdateReview.UpdateReviewCommand
        {
            Id = id,
            AuthorId = request.AuthorId,
            MovieId = request.MovieId,
            Stars = request.Stars
        });

        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteReview([FromRoute] Guid id, [FromServices] ISender sender)
    {
        _ = await sender.Send(new Commands.DeleteReview.DeleteReviewCommand
        {
            Id = id,
        });

        return TypedResults.NoContent();
    }
}

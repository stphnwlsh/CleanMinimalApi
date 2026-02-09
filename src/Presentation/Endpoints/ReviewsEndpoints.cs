namespace CleanMinimalApi.Presentation.Endpoints;

using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Presentation.Filters;
using CleanMinimalApi.Presentation.Requests;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Commands = Application.Reviews.Commands;
using Entities = Application.Reviews.Entities;
using Queries = Application.Reviews.Queries;

public static class ReviewsEndpoints
{
    public static WebApplication MapReviewEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/review")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("review")
            .WithDescription("Lookup, Find and Manipulate Reviews");

        _ = root.MapGet("/", GetReviews)
            .Produces<List<Entities.Review>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Reviews")
            .WithDescription("\n    GET /review");

        _ = root.MapGet("/{id}", GetReviewById)
            .Produces<Entities.Review>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithSummary("Lookup a Review by its Ids")
            .WithDescription("\n    GET /review/00000000-0000-0000-0000-000000000000");

        _ = root.MapPost("/", CreateReview)
            .Produces<Entities.Review>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithSummary("Create a Review")
            .WithDescription("\n    POST /review\n     {         \"authorId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"movieId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"stars\": 5       }");

        _ = root.MapDelete("/{id}", DeleteReview)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithSummary("Delete a Review by its Id")
            .WithDescription("\n    DELETE /review/00000000-0000-0000-0000-000000000000");

        _ = root.MapPut("/{id}", UpdateReview)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithSummary("Update a Review")
            .WithDescription("\n    PUT /review/00000000-0000-0000-0000-000000000000\n     {         \"authorId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"movieId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"stars\": 5       }");

        return app;
    }

    public static async Task<Results<Ok<List<Entities.Review>>, ProblemHttpResult>> GetReviews([FromServices] IMediator mediator)
    {
        try
        {
            return TypedResults.Ok(await mediator.Send(new Queries.GetReviews.GetReviewsQuery()));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<Results<Ok<Entities.Review>, NotFound<string>, ProblemHttpResult>> GetReviewById([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        try
        {
            return TypedResults.Ok(await mediator.Send(new Queries.GetReviewById.GetReviewByIdQuery
            {
                Id = id
            }));
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<Results<Created<Entities.Review>, NotFound<string>, ProblemHttpResult>> CreateReview([Validate][FromBody] CreateReviewRequest request, [FromServices] IMediator mediator)
    {
        try
        {
            var response = await mediator.Send(new Commands.CreateReview.CreateReviewCommand
            {
                AuthorId = request.AuthorId,
                MovieId = request.MovieId,
                Stars = request.Stars
            });

            return TypedResults.Created($"/api/review/{response.Id}", response);
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<Results<NoContent, NotFound<string>, ProblemHttpResult>> UpdateReview([Validate][FromRoute] Guid id, [Validate][FromBody] UpdateReviewRequest request, [FromServices] IMediator mediator)
    {
        try
        {
            _ = await mediator.Send(new Commands.UpdateReview.UpdateReviewCommand
            {
                Id = id,
                AuthorId = request.AuthorId,
                MovieId = request.MovieId,
                Stars = request.Stars
            });

            return TypedResults.NoContent();
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<Results<NoContent, NotFound<string>, ProblemHttpResult>> DeleteReview([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        try
        {
            _ = await mediator.Send(new Commands.DeleteReview.DeleteReviewCommand
            {
                Id = id,
            });

            return TypedResults.NoContent();
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

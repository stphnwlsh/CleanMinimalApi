namespace CleanMinimalApi.Presentation.Endpoints;

using Application.Common.Paging;
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
        var root = app.MapGroup("/api/reviews")
            .WithTags("Reviews")
            .RequireAuthorization();

        _ = root.MapGet("/", GetReviews)
            .WithName(nameof(GetReviews))
            .Produces<Page<Entities.Review>>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        _ = root.MapGet("/{id:guid}", GetReviewById)
            .WithName(nameof(GetReviewById))
            .Produces<Entities.Review>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        _ = root.MapPost("/", CreateReview)
            .WithName(nameof(CreateReview))
            .Produces<Entities.Review>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        _ = root.MapDelete("/{id:guid}", DeleteReview)
            .WithName(nameof(DeleteReview))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        _ = root.MapPut("/{id:guid}", UpdateReview)
            .WithName(nameof(UpdateReview))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    /// <summary>
    /// List Reviews
    /// </summary>
    /// <remarks>
    /// Returns one page of reviews, ordered by movie then author, along with the movie each is about and the
    /// author who wrote it. Optionally return only the reviews of one movie, by one author, or both. To fetch
    /// the next page, add the limit to the offset.
    ///
    /// Sample request:
    ///
    ///     GET /api/reviews?movieId=3fa85f64-5717-4562-b3fc-2c963f66afa6&amp;offset=0&amp;limit=10
    /// </remarks>
    /// <param name="movieId">Only return reviews of this movie. Leave it out for every movie.</param>
    /// <param name="authorId">Only return reviews by this author. Leave it out for every author.</param>
    /// <param name="offset">The number of reviews to skip, 0 for the first page.</param>
    /// <param name="limit">The largest number of reviews to return, from 1 to 50.</param>
    /// <response code="200">One page of Reviews.</response>
    /// <response code="400">An Id, the offset or the limit is not valid.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Page<Entities.Review>>> GetReviews(
        [FromQuery] Guid? movieId,
        [FromQuery] Guid? authorId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = Paging.DefaultLimit)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetReviews.GetReviewsQuery
        {
            MovieId = movieId,
            AuthorId = authorId,
            Offset = offset,
            Limit = limit
        }, cancellationToken));
    }

    /// <summary>
    /// Look up a Review by its Id
    /// </summary>
    /// <remarks>
    /// Returns a single review along with the movie it is about and the author who wrote it.
    ///
    /// Sample request:
    ///
    ///     GET /api/reviews/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </remarks>
    /// <param name="id">The unique identifier of the review.</param>
    /// <response code="200">The Review with the specified Id.</response>
    /// <response code="400">The Id is not valid.</response>
    /// <response code="404">No Review exists with the specified Id.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Entities.Review>> GetReviewById(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetReviewById.GetReviewByIdQuery
        {
            Id = id
        }, cancellationToken));
    }

    /// <summary>
    /// Create a Review
    /// </summary>
    /// <remarks>
    /// Creates a review of a movie by an author. Both the author and the movie must already exist,
    /// and the star rating must be between 1 and 5.
    ///
    /// Sample request:
    ///
    ///     POST /api/reviews
    ///     {
    ///         "authorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "movieId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "stars": 5
    ///     }
    /// </remarks>
    /// <param name="request">The review to create.</param>
    /// <response code="201">The Review that was created. The Location header holds its URL.</response>
    /// <response code="400">The request is not valid, for example the star rating is out of range.</response>
    /// <response code="404">The author or the movie does not exist.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<CreatedAtRoute<Entities.Review>> CreateReview(
        [FromBody] CreateReviewRequest request,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(new Commands.CreateReview.CreateReviewCommand
        {
            AuthorId = request.AuthorId,
            MovieId = request.MovieId,
            Stars = request.Stars
        }, cancellationToken);

        return TypedResults.CreatedAtRoute(response, nameof(GetReviewById), new { id = response.Id });
    }

    /// <summary>
    /// Update the star rating of a Review
    /// </summary>
    /// <remarks>
    /// Changes the star rating of an existing review, which must be between 1 and 5. The author and the
    /// movie of a review can't change. To review a different movie, create a new review.
    ///
    /// Sample request:
    ///
    ///     PUT /api/reviews/3fa85f64-5717-4562-b3fc-2c963f66afa6
    ///     {
    ///         "stars": 5
    ///     }
    /// </remarks>
    /// <param name="id">The unique identifier of the review.</param>
    /// <param name="request">The new star rating of the review.</param>
    /// <response code="204">The Review was updated.</response>
    /// <response code="400">The request is not valid, for example the star rating is out of range.</response>
    /// <response code="404">No Review exists with the specified Id.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<NoContent> UpdateReview(
        [FromRoute] Guid id,
        [FromBody] UpdateReviewRequest request,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        _ = await sender.Send(new Commands.UpdateReview.UpdateReviewCommand
        {
            Id = id,
            Stars = request.Stars
        }, cancellationToken);

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Delete a Review by its Id
    /// </summary>
    /// <remarks>
    /// Permanently removes a review.
    ///
    /// Sample request:
    ///
    ///     DELETE /api/reviews/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </remarks>
    /// <param name="id">The unique identifier of the review.</param>
    /// <response code="204">The Review was deleted.</response>
    /// <response code="400">The Id is not valid.</response>
    /// <response code="404">No Review exists with the specified Id.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<NoContent> DeleteReview(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        _ = await sender.Send(new Commands.DeleteReview.DeleteReviewCommand
        {
            Id = id,
        }, cancellationToken);

        return TypedResults.NoContent();
    }
}

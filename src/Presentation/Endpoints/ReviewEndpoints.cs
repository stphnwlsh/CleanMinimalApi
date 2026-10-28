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
        var root = app.MapGroup("/api/review")
            .WithTags("Reviews");

        _ = root.MapGet("/", GetReviews)
            .WithName(nameof(GetReviews))
            .Produces<List<Entities.Review>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        _ = root.MapGet("/search", SearchReviews)
            .WithName(nameof(SearchReviews))
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
    /// Lookup all Reviews
    /// </summary>
    /// <remarks>
    /// Returns every review along with the movie it is about and the author who wrote it.
    ///
    /// Sample request:
    ///
    ///     GET /api/review
    /// </remarks>
    /// <response code="200">A list of all the Reviews.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<List<Entities.Review>>> GetReviews([FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetReviews.GetReviewsQuery()));
    }

    /// <summary>
    /// Search Reviews by movie or author
    /// </summary>
    /// <remarks>
    /// Returns one page of reviews, optionally only those of one movie, by one author, or both,
    /// ordered by movie then author. When more results remain, the response holds a nextCursor.
    /// Send it back as the cursor to fetch the next page.
    ///
    /// Sample request:
    ///
    ///     GET /api/review/search?movieId=3fa85f64-5717-4562-b3fc-2c963f66afa6&amp;limit=10
    /// </remarks>
    /// <param name="movieId">Only return reviews of this movie. Leave it out for every movie.</param>
    /// <param name="authorId">Only return reviews by this author. Leave it out for every author.</param>
    /// <param name="limit">The largest number of reviews to return, from 1 to 50.</param>
    /// <param name="cursor">The nextCursor from a previous page. Leave it out for the first page.</param>
    /// <response code="200">One page of matching Reviews.</response>
    /// <response code="400">An Id, the limit or the cursor is not valid.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Page<Entities.Review>>> SearchReviews(
        [FromQuery] Guid? movieId,
        [FromQuery] Guid? authorId,
        [FromServices] ISender sender,
        [FromQuery] int limit = Paging.DefaultLimit,
        [FromQuery] string cursor = null)
    {
        return TypedResults.Ok(await sender.Send(new Queries.SearchReviews.SearchReviewsQuery
        {
            MovieId = movieId,
            AuthorId = authorId,
            Limit = limit,
            Cursor = cursor
        }));
    }

    /// <summary>
    /// Lookup a Review by its Id
    /// </summary>
    /// <remarks>
    /// Returns a single review along with the movie it is about and the author who wrote it.
    ///
    /// Sample request:
    ///
    ///     GET /api/review/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </remarks>
    /// <param name="id">The unique identifier of the review.</param>
    /// <response code="200">The Review with the specified Id.</response>
    /// <response code="400">The Id is not valid.</response>
    /// <response code="404">No Review exists with the specified Id.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<Ok<Entities.Review>> GetReviewById([FromRoute] Guid id, [FromServices] ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new Queries.GetReviewById.GetReviewByIdQuery
        {
            Id = id
        }));
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
    ///     POST /api/review
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

    /// <summary>
    /// Update a Review by its Id
    /// </summary>
    /// <remarks>
    /// Replaces the author, movie and star rating of an existing review. The author and the movie
    /// must already exist, and the star rating must be between 1 and 5.
    ///
    /// Sample request:
    ///
    ///     PUT /api/review/3fa85f64-5717-4562-b3fc-2c963f66afa6
    ///     {
    ///         "authorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "movieId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "stars": 5
    ///     }
    /// </remarks>
    /// <param name="id">The unique identifier of the review.</param>
    /// <param name="request">The new details of the review.</param>
    /// <response code="204">The Review was updated.</response>
    /// <response code="400">The request is not valid, for example the star rating is out of range.</response>
    /// <response code="404">The review, the author or the movie does not exist.</response>
    /// <response code="500">An unexpected error occurred.</response>
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

    /// <summary>
    /// Delete a Review by its Id
    /// </summary>
    /// <remarks>
    /// Permanently removes a review.
    ///
    /// Sample request:
    ///
    ///     DELETE /api/review/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// </remarks>
    /// <param name="id">The unique identifier of the review.</param>
    /// <response code="204">The Review was deleted.</response>
    /// <response code="400">The Id is not valid.</response>
    /// <response code="404">No Review exists with the specified Id.</response>
    /// <response code="500">An unexpected error occurred.</response>
    public static async Task<NoContent> DeleteReview([FromRoute] Guid id, [FromServices] ISender sender)
    {
        _ = await sender.Send(new Commands.DeleteReview.DeleteReviewCommand
        {
            Id = id,
        });

        return TypedResults.NoContent();
    }
}

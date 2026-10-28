namespace CleanMinimalApi.Presentation.Requests;

/// <summary>
/// The details needed to update a review.
/// </summary>
public class UpdateReviewRequest
{
    /// <summary>
    /// The unique identifier of the author writing the review.
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid AuthorId { get; init; }

    /// <summary>
    /// The unique identifier of the movie being reviewed.
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid MovieId { get; init; }

    /// <summary>
    /// The star rating, from 1 to 5.
    /// </summary>
    /// <example>5</example>
    public int Stars { get; init; }
}

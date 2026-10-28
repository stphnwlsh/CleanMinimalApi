namespace CleanMinimalApi.Presentation.Requests;

/// <summary>
/// The new star rating of a review. The author and the movie of a review can't change.
/// </summary>
public class UpdateReviewRequest
{
    /// <summary>
    /// The star rating, from 1 to 5.
    /// </summary>
    /// <example>5</example>
    public int Stars { get; init; }
}

namespace CleanMinimalApi.Application.Reviews.Commands.UpdateReview;

using Mediator;

/// <summary>
/// Changes the star rating of a review. The author and the movie of a review can't change.
/// </summary>
public class UpdateReviewCommand : IRequest<bool>
{
    /// <summary>The unique identifier of the review.</summary>
    public Guid Id { get; init; }

    /// <summary>The new star rating, from 1 to 5.</summary>
    public int Stars { get; init; }
}

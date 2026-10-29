namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewsForAuthors;

using Entities;
using Mediator;

/// <summary>
/// Gets every review of several authors in one call, grouped by author. Presentation layers use it to load reviews for a
/// whole page of authors at once instead of one query per author.
/// </summary>
public class GetReviewsForAuthorsQuery : IRequest<Dictionary<Guid, List<Review>>>
{
    /// <summary>The authors to get the reviews of.</summary>
    public required IReadOnlyCollection<Guid> AuthorIds { get; init; }
}

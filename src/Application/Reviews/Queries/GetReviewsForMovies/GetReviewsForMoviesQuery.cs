namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewsForMovies;

using Entities;
using Mediator;

/// <summary>
/// Gets every review of several movies in one call, grouped by movie. Presentation layers use it to load reviews for a
/// whole page of movies at once instead of one query per movie.
/// </summary>
public class GetReviewsForMoviesQuery : IRequest<Dictionary<Guid, List<Review>>>
{
    /// <summary>The movies to get the reviews of.</summary>
    public required IReadOnlyCollection<Guid> MovieIds { get; init; }
}

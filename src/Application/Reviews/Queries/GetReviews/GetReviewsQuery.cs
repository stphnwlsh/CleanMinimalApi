namespace CleanMinimalApi.Application.Reviews.Queries.GetReviews;

using Common.Paging;
using Entities;
using Mediator;

/// <summary>
/// Lists reviews one page at a time, optionally only those of one movie, by one author, or both.
/// </summary>
public class GetReviewsQuery : IRequest<Page<Review>>
{
    /// <summary>Only return reviews of this movie. Leave empty for every movie.</summary>
    public Guid? MovieId { get; init; }

    /// <summary>Only return reviews by this author. Leave empty for every author.</summary>
    public Guid? AuthorId { get; init; }

    /// <summary>The number of results to skip, 0 for the first page.</summary>
    public int Offset { get; init; }

    /// <summary>The largest number of results to return, from 1 to 50.</summary>
    public int Limit { get; init; } = Paging.DefaultLimit;
}

namespace CleanMinimalApi.Application.Reviews.Queries.SearchReviews;

using Common.Paging;
using Entities;
using Mediator;

/// <summary>
/// Finds reviews, optionally only those for one movie or by one author, one page at a time.
/// </summary>
public class SearchReviewsQuery : IRequest<Page<Review>>
{
    /// <summary>Only return reviews of this movie. Leave empty for every movie.</summary>
    public Guid? MovieId { get; init; }

    /// <summary>Only return reviews by this author. Leave empty for every author.</summary>
    public Guid? AuthorId { get; init; }

    /// <summary>The largest number of results to return, from 1 to 50.</summary>
    public int Limit { get; init; } = Paging.DefaultLimit;

    /// <summary>The cursor from a previous page, or empty for the first page.</summary>
    public string Cursor { get; init; }
}

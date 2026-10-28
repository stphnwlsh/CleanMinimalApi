namespace CleanMinimalApi.Application.Movies.Queries.SearchMovies;

using Common.Paging;
using Entities;
using Mediator;

/// <summary>
/// Finds movies whose the title contains a search term, one page at a time.
/// </summary>
public class SearchMoviesQuery : IRequest<Page<Movie>>
{
    /// <summary>The words to look for, ignoring case. Leave empty to list every one.</summary>
    public string Query { get; init; }

    /// <summary>The largest number of results to return, from 1 to 50.</summary>
    public int Limit { get; init; } = Paging.DefaultLimit;

    /// <summary>The cursor from a previous page, or empty for the first page.</summary>
    public string Cursor { get; init; }
}

namespace CleanMinimalApi.Application.Movies.Queries.GetMovies;

using Common.Paging;
using Entities;
using Mediator;

/// <summary>
/// Lists movies one page at a time, optionally only those whose title contains a search term.
/// </summary>
public class GetMoviesQuery : IRequest<Page<Movie>>
{
    /// <summary>The words to look for in the title, ignoring case. Leave empty to list every movie.</summary>
    public string? Query { get; init; }

    /// <summary>The number of results to skip, 0 for the first page.</summary>
    public int Offset { get; init; }

    /// <summary>The largest number of results to return, from 1 to 50.</summary>
    public int Limit { get; init; } = Paging.DefaultLimit;
}

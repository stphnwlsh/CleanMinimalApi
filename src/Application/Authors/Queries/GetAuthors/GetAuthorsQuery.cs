namespace CleanMinimalApi.Application.Authors.Queries.GetAuthors;

using Common.Paging;
using Entities;
using Mediator;

/// <summary>
/// Lists authors one page at a time, optionally only those whose name contains a search term.
/// </summary>
public class GetAuthorsQuery : IRequest<Page<Author>>
{
    /// <summary>The words to look for in the first, last or full name, ignoring case. Leave empty to list every author.</summary>
    public string? Query { get; init; }

    /// <summary>The number of results to skip, 0 for the first page.</summary>
    public int Offset { get; init; }

    /// <summary>The largest number of results to return, from 1 to 50.</summary>
    public int Limit { get; init; } = Paging.DefaultLimit;
}

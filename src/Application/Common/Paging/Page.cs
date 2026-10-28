namespace CleanMinimalApi.Application.Common.Paging;

/// <summary>
/// One page of results from a search.
/// </summary>
/// <typeparam name="T">The type of the results.</typeparam>
/// <param name="Items">The results on this page.</param>
/// <param name="TotalCount">The number of results across every page.</param>
/// <param name="NextCursor">The cursor that fetches the next page, or null when this is the last page.</param>
public record Page<T>(List<T> Items, int TotalCount, string NextCursor = null);

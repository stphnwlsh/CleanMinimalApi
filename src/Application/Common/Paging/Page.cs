namespace CleanMinimalApi.Application.Common.Paging;

/// <summary>
/// One page of results from a list.
/// </summary>
/// <typeparam name="T">The type of the results.</typeparam>
/// <param name="Items">The results on this page.</param>
/// <param name="Offset">The number of results skipped before this page.</param>
/// <param name="Limit">The largest number of results this page could hold.</param>
/// <param name="TotalCount">The number of results across every page.</param>
public record Page<T>(List<T> Items, int Offset, int Limit, int TotalCount);

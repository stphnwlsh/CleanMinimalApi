namespace CleanMinimalApi.Application.Common.Paging;

/// <summary>
/// The limits shared by every paged list.
/// </summary>
public static class Paging
{
    /// <summary>The number of results returned when no limit is supplied.</summary>
    public const int DefaultLimit = 10;

    /// <summary>The largest number of results a single page can return.</summary>
    public const int MaxLimit = 50;
}

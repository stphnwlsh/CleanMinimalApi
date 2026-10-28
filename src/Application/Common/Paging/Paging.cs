namespace CleanMinimalApi.Application.Common.Paging;

using System.Globalization;
using System.Text;

/// <summary>
/// Limits and opaque cursors shared by every search.
/// </summary>
public static class Paging
{
    /// <summary>The number of results returned when no limit is supplied.</summary>
    public const int DefaultLimit = 10;

    /// <summary>The largest number of results a single page can return.</summary>
    public const int MaxLimit = 50;

    private const string Prefix = "offset:";

    /// <summary>Creates the cursor for the page that starts at <paramref name="offset"/>.</summary>
    /// <param name="offset">The number of results to skip.</param>
    /// <returns>An opaque cursor.</returns>
    public static string Encode(int offset)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(Prefix + offset.ToString(CultureInfo.InvariantCulture)));
    }

    /// <summary>Reads the offset from a cursor. An empty cursor is the first page.</summary>
    /// <param name="cursor">The cursor from a previous page, or null for the first page.</param>
    /// <param name="offset">The number of results to skip.</param>
    /// <returns>True when the cursor is empty or valid.</returns>
    public static bool TryDecode(string cursor, out int offset)
    {
        offset = 0;

        if (string.IsNullOrEmpty(cursor))
        {
            return true;
        }

        try
        {
            var value = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));

            return value.StartsWith(Prefix, StringComparison.Ordinal)
                && int.TryParse(value.AsSpan(Prefix.Length), NumberStyles.None, CultureInfo.InvariantCulture, out offset);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    /// <summary>Creates the page for a set of results, with a cursor when more results remain.</summary>
    /// <typeparam name="T">The type of the results.</typeparam>
    /// <param name="items">The results on this page.</param>
    /// <param name="totalCount">The number of results across every page.</param>
    /// <param name="offset">The number of results skipped before this page.</param>
    /// <returns>The page.</returns>
    public static Page<T> ToPage<T>(List<T> items, int totalCount, int offset)
    {
        var next = offset + items.Count;

        return new Page<T>(items, totalCount, next < totalCount ? Encode(next) : null);
    }
}

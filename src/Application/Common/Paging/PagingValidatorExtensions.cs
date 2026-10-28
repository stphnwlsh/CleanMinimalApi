namespace CleanMinimalApi.Application.Common.Paging;

using FluentValidation;

/// <summary>
/// Validation rules shared by every search.
/// </summary>
public static class PagingValidatorExtensions
{
    /// <summary>Checks that a limit is between 1 and <see cref="Paging.MaxLimit"/>.</summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="rule">The rule for the limit.</param>
    /// <returns>The rule.</returns>
    public static IRuleBuilderOptions<T, int> ValidLimit<T>(this IRuleBuilder<T, int> rule)
    {
        return rule.InclusiveBetween(1, Paging.MaxLimit).WithMessage($"A limit must be between 1 and {Paging.MaxLimit}.");
    }

    /// <summary>Checks that a cursor is empty or came from a previous page.</summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="rule">The rule for the cursor.</param>
    /// <returns>The rule.</returns>
    public static IRuleBuilderOptions<T, string> ValidCursor<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.Must(cursor => Paging.TryDecode(cursor, out _))
            .WithMessage("The cursor is not valid. Use the nextCursor from a previous page, or leave it empty for the first page.");
    }

    /// <summary>Checks that a search term is no longer than 100 characters.</summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="rule">The rule for the search term.</param>
    /// <returns>The rule.</returns>
    public static IRuleBuilderOptions<T, string> ValidSearchTerm<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.MaximumLength(100).WithMessage("A search term can't be longer than 100 characters.");
    }
}

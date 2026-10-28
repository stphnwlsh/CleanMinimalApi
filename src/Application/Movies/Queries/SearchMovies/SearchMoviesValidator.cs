namespace CleanMinimalApi.Application.Movies.Queries.SearchMovies;

using Common.Paging;
using FluentValidation;

public class SearchMoviesValidator : AbstractValidator<SearchMoviesQuery>
{
    public SearchMoviesValidator()
    {
        _ = this.RuleFor(r => r.Query).ValidSearchTerm();
        _ = this.RuleFor(r => r.Limit).ValidLimit();
        _ = this.RuleFor(r => r.Cursor).ValidCursor();
    }
}

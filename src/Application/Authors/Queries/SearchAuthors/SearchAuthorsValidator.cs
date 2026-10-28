namespace CleanMinimalApi.Application.Authors.Queries.SearchAuthors;

using Common.Paging;
using FluentValidation;

public class SearchAuthorsValidator : AbstractValidator<SearchAuthorsQuery>
{
    public SearchAuthorsValidator()
    {
        _ = this.RuleFor(r => r.Query).ValidSearchTerm();
        _ = this.RuleFor(r => r.Limit).ValidLimit();
        _ = this.RuleFor(r => r.Cursor).ValidCursor();
    }
}

namespace CleanMinimalApi.Application.Authors.Queries.GetAuthors;

using Common.Paging;
using FluentValidation;

public class GetAuthorsValidator : AbstractValidator<GetAuthorsQuery>
{
    public GetAuthorsValidator()
    {
        _ = this.RuleFor(r => r.Query).ValidSearchTerm();
        _ = this.RuleFor(r => r.Offset).ValidOffset();
        _ = this.RuleFor(r => r.Limit).ValidLimit();
    }
}

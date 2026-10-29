namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewsForAuthors;

using Common.Paging;
using FluentValidation;

public class GetReviewsForAuthorsValidator : AbstractValidator<GetReviewsForAuthorsQuery>
{
    public GetReviewsForAuthorsValidator()
    {
        _ = this.RuleFor(r => r.AuthorIds)
            .NotEmpty().WithMessage("At least one Author Id is needed.")
            .Must(ids => ids.Count <= Paging.MaxLimit).WithMessage($"Reviews can be fetched for at most {Paging.MaxLimit} authors at a time.");
        _ = this.RuleForEach(r => r.AuthorIds).NotEqual(Guid.Empty).WithMessage("An Author Id can't be empty.");
    }
}

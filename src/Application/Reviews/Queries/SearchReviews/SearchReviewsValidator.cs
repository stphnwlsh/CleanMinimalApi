namespace CleanMinimalApi.Application.Reviews.Queries.SearchReviews;

using Common.Paging;
using FluentValidation;

public class SearchReviewsValidator : AbstractValidator<SearchReviewsQuery>
{
    public SearchReviewsValidator()
    {
        _ = this.RuleFor(r => r.MovieId).NotEqual(Guid.Empty).WithMessage("A Movie Id can't be empty. Leave it out to search every movie.");
        _ = this.RuleFor(r => r.AuthorId).NotEqual(Guid.Empty).WithMessage("An Author Id can't be empty. Leave it out to search every author.");
        _ = this.RuleFor(r => r.Limit).ValidLimit();
        _ = this.RuleFor(r => r.Cursor).ValidCursor();
    }
}

namespace CleanMinimalApi.Application.Reviews.Queries.GetReviews;

using Common.Paging;
using FluentValidation;

public class GetReviewsValidator : AbstractValidator<GetReviewsQuery>
{
    public GetReviewsValidator()
    {
        _ = this.RuleFor(r => r.MovieId).NotEqual(Guid.Empty).WithMessage("A Movie Id can't be empty. Leave it out to list every movie.");
        _ = this.RuleFor(r => r.AuthorId).NotEqual(Guid.Empty).WithMessage("An Author Id can't be empty. Leave it out to list every author.");
        _ = this.RuleFor(r => r.Offset).ValidOffset();
        _ = this.RuleFor(r => r.Limit).ValidLimit();
    }
}

namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewsForMovies;

using Common.Paging;
using FluentValidation;

public class GetReviewsForMoviesValidator : AbstractValidator<GetReviewsForMoviesQuery>
{
    public GetReviewsForMoviesValidator()
    {
        _ = this.RuleFor(r => r.MovieIds)
            .NotEmpty().WithMessage("At least one Movie Id is needed.")
            .Must(ids => ids.Count <= Paging.MaxLimit).WithMessage($"Reviews can be fetched for at most {Paging.MaxLimit} movies at a time.");
        _ = this.RuleForEach(r => r.MovieIds).NotEqual(Guid.Empty).WithMessage("A Movie Id can't be empty.");
    }
}

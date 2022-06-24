namespace CleanMinimalApi.Application.Reviews.Commands.CreateReview;

using FluentValidation;

public class CreateReviewValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewValidator()
    {
        _ = this.RuleFor(r => r.AuthorId).NotEqual(Guid.Empty).WithMessage("An author id was not supplied to create the review.");
        _ = this.RuleFor(r => r.MovieId).NotEqual(Guid.Empty).WithMessage("A movie id was not supplied to create the review.");
        _ = this.RuleFor(r => r.Stars).InclusiveBetween(1, 5).WithMessage("A star rating must be between 1 and 5.");
    }
}

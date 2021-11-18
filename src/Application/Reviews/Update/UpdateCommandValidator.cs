namespace CleanMinimalApi.Application.Reviews.Update;

using FluentValidation;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        _ = this.RuleFor(r => r.Id).NotEqual(Guid.Empty).WithMessage("A review id was not supplied to Update the review.");
        _ = this.RuleFor(r => r.AuthorId).NotEqual(Guid.Empty).WithMessage("An author id was not supplied to Update the review.");
        _ = this.RuleFor(r => r.MovieId).NotEqual(Guid.Empty).WithMessage("A movie id was not supplied to Update the review.");
        _ = this.RuleFor(r => r.Stars).InclusiveBetween(1, 5).WithMessage("A star rating must be between 1 and 5.");
    }
}

namespace CleanMinimalApi.Application.Reviews.Commands.DeleteReview;

using FluentValidation;

public class DeleteReviewValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewValidator()
    {
        _ = this.RuleFor(r => r.Id).NotEqual(Guid.Empty).WithMessage("A review Id was not supplied.");
    }
}

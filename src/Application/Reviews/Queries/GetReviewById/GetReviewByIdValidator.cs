namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewById;

using FluentValidation;

public class GetReviewByIdValidator : AbstractValidator<GetReviewByIdQuery>
{
    public GetReviewByIdValidator()
    {
        _ = this.RuleFor(r => r.Id).NotNull().NotEqual(Guid.Empty).WithMessage("A Review Id was not supplied.");
    }
}

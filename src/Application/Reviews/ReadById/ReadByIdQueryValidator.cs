namespace CleanMinimalApi.Application.Reviews.ReadById;

using FluentValidation;

public class ReadByIdQueryValidator : AbstractValidator<ReadByIdQuery>
{
    public ReadByIdQueryValidator()
    {
        _ = this.RuleFor(r => r.Id).NotNull().NotEqual(Guid.Empty).WithMessage("A Review Id was not supplied.");
    }
}

namespace CleanMinimalApi.Application.Authors.Update;

using FluentValidation;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        _ = this.RuleFor(r => r.Id).NotEqual(Guid.Empty).WithMessage("A review id was not supplied to Update the review.");
        _ = this.RuleFor(r => r.FirstName).NotNull().NotEmpty().WithMessage("A FirstName was not supplied to create the author.");
        _ = this.RuleFor(r => r.LastName).NotNull().NotEmpty().WithMessage("A LastName was not supplied to create the author.");
    }
}

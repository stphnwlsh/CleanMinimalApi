namespace CleanMinimalApi.Application.Authors.Create;

using FluentValidation;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        _ = this.RuleFor(r => r.FirstName).NotNull().NotEmpty().WithMessage("A FirstName was not supplied to create the author.");
        _ = this.RuleFor(r => r.LastName).NotNull().NotEmpty().WithMessage("A LastName was not supplied to create the author.");
    }
}

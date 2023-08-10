namespace CleanMinimalApi.Presentation.Validators;

using FluentValidation;

public class GenericIdentityValidator : AbstractValidator<Guid>
{
    public GenericIdentityValidator()
    {
        _ = this.RuleFor(r => r).NotEqual(Guid.Empty).WithMessage("A valid Id was not supplied.");
    }
}

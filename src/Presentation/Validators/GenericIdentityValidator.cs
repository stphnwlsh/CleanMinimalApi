namespace CleanMinimalApi.Presentation.Validators;

using FluentValidation;

public class GenericIdentityValidator : AbstractValidator<Guid>
{
    public GenericIdentityValidator()
    {
        _ = this.RuleFor(r => r)
            .NotNull()
            .NotEqual(Guid.Empty)
            .WithMessage("The Id supplied in the request is not valid.");
    }
}

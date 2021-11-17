namespace CleanMinimalApi.Application.Movies.ReadById;

using FluentValidation;

public class ReadByIdQueryValidator : AbstractValidator<ReadByIdQuery>
{
    public ReadByIdQueryValidator()
    {
        _ = this.RuleFor(r => r.Id).NotEqual(Guid.Empty).WithMessage("A Movie Id was not supplied.");
    }
}

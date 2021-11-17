namespace CleanMinimalApi.Application.Authors.ReadById;

using FluentValidation;

public class ReadByIdQueryValidator : AbstractValidator<ReadByIdQuery>
{
    public ReadByIdQueryValidator()
    {
        _ = this.RuleFor(r => r.Id).NotNull().NotEqual(Guid.Empty).WithMessage("An Author Id was not supplied.");
    }
}

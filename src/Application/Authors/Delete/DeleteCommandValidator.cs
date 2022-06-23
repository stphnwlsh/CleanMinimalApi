namespace CleanMinimalApi.Application.Authors.Delete;

using FluentValidation;

public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
{
    public DeleteCommandValidator() => _ = this.RuleFor(r => r.Id).NotEqual(Guid.Empty).WithMessage("An author Id was not supplied.");
}

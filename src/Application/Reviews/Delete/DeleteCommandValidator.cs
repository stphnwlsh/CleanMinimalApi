namespace CleanMinimalApi.Application.Reviews.Delete;

using FluentValidation;

public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
{
    public DeleteCommandValidator()
    {
        _ = this.RuleFor(r => r.Id).NotEqual(Guid.Empty).WithMessage("A review Id was not supplied.");
    }
}

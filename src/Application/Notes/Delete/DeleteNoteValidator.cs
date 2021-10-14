namespace CleanMinimalApi.Application.Notes.Delete;

using FluentValidation;

public class DeleteNoteValidator : AbstractValidator<DeleteNoteCommand>
{
    public DeleteNoteValidator()
    {
        _ = this.RuleFor(v => v.Id)
            .GreaterThan(0);
    }
}

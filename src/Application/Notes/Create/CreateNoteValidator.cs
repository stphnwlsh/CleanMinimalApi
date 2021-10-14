namespace CleanMinimalApi.Application.Notes.Create;

using FluentValidation;

public class CreateNoteValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteValidator()
    {
        _ = this.RuleFor(v => v.Text)
            .NotEqual("Invalid")
            .MaximumLength(300)
            .NotEmpty();
    }
}

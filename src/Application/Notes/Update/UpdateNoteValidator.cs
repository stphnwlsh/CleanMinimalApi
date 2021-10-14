namespace CleanMinimalApi.Application.Notes.Update;

using FluentValidation;

public class UpdateNoteValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteValidator()
    {
        _ = this.RuleFor(v => v.Id)
            .GreaterThan(0);

        _ = this.RuleFor(v => v.Text)
            .NotEqual("Invalid")
            .MaximumLength(300)
            .NotEmpty();
    }
}

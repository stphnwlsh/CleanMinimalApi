namespace CleanMinimalApi.Application.Notes.Lookup;

using FluentValidation;

public class LookupNoteValidator : AbstractValidator<LookupNoteQuery>
{
    public LookupNoteValidator()
    {
        _ = this.RuleFor(v => v.Id)
            .GreaterThan(0);
    }
}

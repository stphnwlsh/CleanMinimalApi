namespace CleanMinimalApi.Application.Movies.GetMovieById;

using FluentValidation;

public class GetMovieByIdValidator : AbstractValidator<GetMovieByIdQuery>
{
    public GetMovieByIdValidator()
    {
        _ = this.RuleFor(r => r.Id).NotEqual(Guid.Empty).WithMessage("A Movie Id was not supplied.");
    }
}

namespace CleanMinimalApi.Application.Authors.Queries.GetAuthorById;

using FluentValidation;

public class GetAuthorByIdValidator : AbstractValidator<GetAuthorByIdQuery>
{
    public GetAuthorByIdValidator()
    {
        _ = this.RuleFor(r => r.Id).NotNull().NotEqual(Guid.Empty).WithMessage("An Author Id was not supplied.");
    }
}

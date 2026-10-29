namespace CleanMinimalApi.Application.Movies.Queries.GetMovies;

using Common.Paging;
using FluentValidation;

public class GetMoviesValidator : AbstractValidator<GetMoviesQuery>
{
    public GetMoviesValidator()
    {
        _ = this.RuleFor(r => r.Query).ValidSearchTerm();
        _ = this.RuleFor(r => r.Offset).ValidOffset();
        _ = this.RuleFor(r => r.Limit).ValidLimit();
    }
}

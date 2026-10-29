namespace CleanMinimalApi.Application.Movies.Queries.GetMovies;

using Common.Paging;
using Entities;
using Mediator;

public class GetMoviesHandler(IMoviesRepository repository) : IRequestHandler<GetMoviesQuery, Page<MovieSummary>>
{
    public async ValueTask<Page<MovieSummary>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await repository.GetMovies(request.Query, request.Offset, request.Limit, cancellationToken);

        return new Page<MovieSummary>(items, request.Offset, request.Limit, totalCount);
    }
}

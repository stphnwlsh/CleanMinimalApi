namespace CleanMinimalApi.Application.Movies.Queries.GetMovies;

using Common.Paging;
using Entities;
using Mediator;

public class GetMoviesHandler(IMoviesRepository repository) : IRequestHandler<GetMoviesQuery, Page<Movie>>
{
    public async ValueTask<Page<Movie>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await repository.GetMovies(request.Query, request.Offset, request.Limit, cancellationToken);

        return new Page<Movie>(items, request.Offset, request.Limit, totalCount);
    }
}

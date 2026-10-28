namespace CleanMinimalApi.Application.Movies.Queries.SearchMovies;

using System.Threading;
using System.Threading.Tasks;
using Common.Paging;
using Entities;
using Mediator;

public class SearchMoviesHandler(IMoviesRepository repository) : IRequestHandler<SearchMoviesQuery, Page<Movie>>
{
    public async ValueTask<Page<Movie>> Handle(SearchMoviesQuery request, CancellationToken cancellationToken)
    {
        _ = Paging.TryDecode(request.Cursor, out var offset);

        var (items, totalCount) = await repository.SearchMovies(request.Query, offset, request.Limit, cancellationToken);

        return Paging.ToPage(items, totalCount, offset);
    }
}

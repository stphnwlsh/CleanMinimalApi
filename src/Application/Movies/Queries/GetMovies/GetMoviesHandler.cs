namespace CleanMinimalApi.Application.Movies.Queries.GetMovies;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using Mediator;

public class GetMoviesHandler(IMoviesRepository repository) : IRequestHandler<GetMoviesQuery, List<Movie>>
{
    public async ValueTask<List<Movie>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetMovies(cancellationToken);
    }
}

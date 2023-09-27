namespace CleanMinimalApi.Application.Movies.Queries.GetMovies;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class GetMoviesHandler(IMoviesRepository repository) : IRequestHandler<GetMoviesQuery, List<Movie>>
{
    public async Task<List<Movie>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetMovies(cancellationToken);
    }
}

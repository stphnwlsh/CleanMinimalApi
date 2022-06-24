namespace CleanMinimalApi.Application.Movies.GetMovies;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Entities;
using MediatR;

public class GetMoviesHandler : IRequestHandler<GetMoviesQuery, List<Movie>>
{
    private readonly IMoviesRepository repository;

    public GetMoviesHandler(IMoviesRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Movie>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        return await this.repository.GetMovies(cancellationToken);
    }
}

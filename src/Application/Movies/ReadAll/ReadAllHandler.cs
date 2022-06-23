namespace CleanMinimalApi.Application.Movies.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Entities;
using MediatR;

public class ReadAllHandler : IRequestHandler<ReadAllQuery, List<Movie>>
{
    private readonly MoviesRepository repository;

    public ReadAllHandler(MoviesRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Movie>> Handle(ReadAllQuery request, CancellationToken cancellationToken)
    {
        return await this.repository.ReadAllMovies(cancellationToken);
    }
}

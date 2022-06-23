namespace CleanMinimalApi.Application.Movies.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Movies;
using Entities;
using MediatR;

public class ReadAllHandler : IRequestHandler<ReadAllQuery, List<Movie>>
{
    private readonly IMoviesRepository repository;

    public ReadAllHandler(IMoviesRepository repository) => this.repository = repository;

    public async Task<List<Movie>> Handle(ReadAllQuery request, CancellationToken cancellationToken) => await this.repository.ReadAllMovies(cancellationToken);
}

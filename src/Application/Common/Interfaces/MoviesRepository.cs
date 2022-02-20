namespace CleanMinimalApi.Application.Common.Interfaces;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Entities;

public interface MoviesRepository
{
    Task<List<Movie>> ReadAllMovies(CancellationToken cancellationToken);
    Task<Movie> ReadMovieById(Guid id, CancellationToken cancellationToken);
    Task<bool> MovieExists(Guid id, CancellationToken cancellationToken);
}

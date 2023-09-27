namespace CleanMinimalApi.Application.Movies;

using System.Threading.Tasks;
using Entities;

public interface IMoviesRepository
{
    Task<List<Movie>> GetMovies(CancellationToken cancellationToken);

    Task<Movie> GetMovieById(Guid id, CancellationToken cancellationToken);

    Task<bool> MovieExists(Guid id, CancellationToken cancellationToken);
}

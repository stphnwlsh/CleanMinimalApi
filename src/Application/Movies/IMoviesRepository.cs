namespace CleanMinimalApi.Application.Movies;

using System.Threading.Tasks;
using Entities;

public interface IMoviesRepository
{
    public Task<List<Movie>> GetMovies(CancellationToken cancellationToken);

    public Task<Movie> GetMovieById(Guid id, CancellationToken cancellationToken);

    public Task<bool> MovieExists(Guid id, CancellationToken cancellationToken);
}

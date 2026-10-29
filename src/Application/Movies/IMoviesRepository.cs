namespace CleanMinimalApi.Application.Movies;

using Entities;

public interface IMoviesRepository
{
    public Task<(List<MovieSummary> Items, int TotalCount)> GetMovies(
        string? query,
        int offset,
        int limit,
        CancellationToken cancellationToken);

    public Task<Movie?> GetMovieById(Guid id, CancellationToken cancellationToken);

    public Task<bool> MovieExists(Guid id, CancellationToken cancellationToken);
}

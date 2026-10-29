namespace CleanMinimalApi.Application.Reviews;

using Entities;

public interface IReviewsRepository
{
    public Task<Review> CreateReview(
        Guid authorId,
        Guid movieId,
        int stars,
        CancellationToken cancellationToken);

    public Task<bool> DeleteReview(Guid id, CancellationToken cancellationToken);

    public Task<(List<Review> Items, int TotalCount)> GetReviews(
        Guid? movieId,
        Guid? authorId,
        int offset,
        int limit,
        CancellationToken cancellationToken);

    public Task<List<Review>> GetReviewsForMovies(IReadOnlyCollection<Guid> movieIds, CancellationToken cancellationToken);

    public Task<List<Review>> GetReviewsForAuthors(IReadOnlyCollection<Guid> authorIds, CancellationToken cancellationToken);

    public Task<Review?> GetReviewById(Guid id, CancellationToken cancellationToken);

    public Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken);

    public Task<bool> UpdateReview(Guid id, int stars, CancellationToken cancellationToken);
}

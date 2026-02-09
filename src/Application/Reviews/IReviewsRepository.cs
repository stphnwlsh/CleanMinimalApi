namespace CleanMinimalApi.Application.Reviews;

using System.Threading.Tasks;
using Entities;

public interface IReviewsRepository
{
    public Task<Review> CreateReview(
        Guid authorId,
        Guid movieId,
        int stars,
        CancellationToken cancellationToken);

    public Task<bool> DeleteReview(Guid id, CancellationToken cancellationToken);

    public Task<List<Review>> GetReviews(CancellationToken cancellationToken);

    public Task<Review> GetReviewById(Guid id, CancellationToken cancellationToken);

    public Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken);

    public Task<bool> UpdateReview(
        Guid id,
        Guid authorId,
        Guid movieId,
        int stars,
        CancellationToken cancellationToken);
}

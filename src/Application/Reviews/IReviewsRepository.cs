namespace CleanMinimalApi.Application.Reviews;

using System.Threading.Tasks;
using Entities;

public interface IReviewsRepository
{
    Task<Review> CreateReview(
        Guid authorId,
        Guid movieId,
        int stars,
        CancellationToken cancellationToken);

    Task<bool> DeleteReview(Guid id, CancellationToken cancellationToken);

    Task<List<Review>> GetReviews(CancellationToken cancellationToken);

    Task<Review> GetReviewById(Guid id, CancellationToken cancellationToken);

    Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken);

    Task<bool> UpdateReview(
        Guid id,
        Guid authorId,
        Guid movieId,
        int stars,
        CancellationToken cancellationToken);
}

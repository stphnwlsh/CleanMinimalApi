namespace CleanMinimalApi.Application.Common.Interfaces;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Entities;

public interface ReviewsRepository
{
    Task<Review> CreateReview(Guid authorId, Guid movieId, int stars, CancellationToken cancellationToken);
    Task<bool> DeleteReview(Guid id, CancellationToken cancellationToken);
    Task<List<Review>> ReadAllReviews(CancellationToken cancellationToken);
    Task<Review> ReadReviewById(Guid id, CancellationToken cancellationToken);
    Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken);
    Task<bool> UpdateReview(Guid id, Guid authorId, Guid movieId, int stars, CancellationToken cancellationToken);
}

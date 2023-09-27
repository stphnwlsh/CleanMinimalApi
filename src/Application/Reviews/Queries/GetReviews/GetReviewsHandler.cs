namespace CleanMinimalApi.Application.Reviews.Queries.GetReviews;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class GetReviewsHandler(IReviewsRepository repository) : IRequestHandler<GetReviewsQuery, List<Review>>
{
    public async Task<List<Review>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetReviews(cancellationToken);
    }
}

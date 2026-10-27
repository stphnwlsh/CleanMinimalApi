namespace CleanMinimalApi.Application.Reviews.Queries.GetReviews;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using Mediator;

public class GetReviewsHandler(IReviewsRepository repository) : IRequestHandler<GetReviewsQuery, List<Review>>
{
    public async ValueTask<List<Review>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetReviews(cancellationToken);
    }
}

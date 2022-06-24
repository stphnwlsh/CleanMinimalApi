namespace CleanMinimalApi.Application.Reviews.Queries.GetReviews;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class GetReviewsHandler : IRequestHandler<GetReviewsQuery, List<Review>>
{
    private readonly IReviewsRepository repository;

    public GetReviewsHandler(IReviewsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Review>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        return await this.repository.GetReviews(cancellationToken);
    }
}

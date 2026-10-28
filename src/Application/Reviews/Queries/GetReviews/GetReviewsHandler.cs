namespace CleanMinimalApi.Application.Reviews.Queries.GetReviews;

using Common.Paging;
using Entities;
using Mediator;

public class GetReviewsHandler(IReviewsRepository repository) : IRequestHandler<GetReviewsQuery, Page<Review>>
{
    public async ValueTask<Page<Review>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await repository.GetReviews(request.MovieId, request.AuthorId, request.Offset, request.Limit, cancellationToken);

        return new Page<Review>(items, request.Offset, request.Limit, totalCount);
    }
}

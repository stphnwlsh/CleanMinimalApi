namespace CleanMinimalApi.Application.Reviews.Queries.SearchReviews;

using System.Threading;
using System.Threading.Tasks;
using Common.Paging;
using Entities;
using Mediator;

public class SearchReviewsHandler(IReviewsRepository repository) : IRequestHandler<SearchReviewsQuery, Page<Review>>
{
    public async ValueTask<Page<Review>> Handle(SearchReviewsQuery request, CancellationToken cancellationToken)
    {
        _ = Paging.TryDecode(request.Cursor, out var offset);

        var (items, totalCount) = await repository.SearchReviews(request.MovieId, request.AuthorId, offset, request.Limit, cancellationToken);

        return Paging.ToPage(items, totalCount, offset);
    }
}

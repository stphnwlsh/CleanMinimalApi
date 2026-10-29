namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewsForAuthors;

using Entities;
using Mediator;

public class GetReviewsForAuthorsHandler(IReviewsRepository repository) : IRequestHandler<GetReviewsForAuthorsQuery, Dictionary<Guid, List<Review>>>
{
    public async ValueTask<Dictionary<Guid, List<Review>>> Handle(GetReviewsForAuthorsQuery request, CancellationToken cancellationToken)
    {
        var reviews = (await repository.GetReviewsForAuthors(request.AuthorIds, cancellationToken))
            .ToLookup(r => r.ReviewAuthor?.Id);

        // Every id gets an entry, so a author with no reviews has an empty list rather than no entry.
        return request.AuthorIds
            .Distinct()
            .ToDictionary(id => id, id => reviews[id].ToList());
    }
}

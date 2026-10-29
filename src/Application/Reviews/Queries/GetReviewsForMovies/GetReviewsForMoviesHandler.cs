namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewsForMovies;

using Entities;
using Mediator;

public class GetReviewsForMoviesHandler(IReviewsRepository repository) : IRequestHandler<GetReviewsForMoviesQuery, Dictionary<Guid, List<Review>>>
{
    public async ValueTask<Dictionary<Guid, List<Review>>> Handle(GetReviewsForMoviesQuery request, CancellationToken cancellationToken)
    {
        var reviews = (await repository.GetReviewsForMovies(request.MovieIds, cancellationToken))
            .ToLookup(r => r.ReviewedMovie?.Id);

        // Every id gets an entry, so a movie with no reviews has an empty list rather than no entry.
        return request.MovieIds
            .Distinct()
            .ToDictionary(id => id, id => reviews[id].ToList());
    }
}

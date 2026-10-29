namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.GetReviewsForMovies;

using System.Threading;
using System.Threading.Tasks;
using Application.Movies.Entities;
using Application.Reviews;
using Application.Reviews.Entities;
using Application.Reviews.Queries.GetReviewsForMovies;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetReviewsForMoviesHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturn_ReviewsGroupedByMovie()
    {
        // Arrange
        var reviewed = Guid.NewGuid();
        var unreviewed = Guid.NewGuid();
        var query = new GetReviewsForMoviesQuery { MovieIds = [reviewed, unreviewed, reviewed] };

        var repository = Substitute.For<IReviewsRepository>();
        var handler = new GetReviewsForMoviesHandler(repository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = repository.GetReviewsForMovies(query.MovieIds, token).Returns(
        [
            new Review(Guid.NewGuid(), 5, new ReviewedMovie(reviewed, "Title")),
            new Review(Guid.NewGuid(), 3, new ReviewedMovie(reviewed, "Title"))
        ]);

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await repository.Received(1).GetReviewsForMovies(query.MovieIds, token);

        result.Count.ShouldBe(2);
        result[reviewed].Select(r => r.Stars).ShouldBe([5, 3]);
        result[unreviewed].ShouldBeEmpty();
    }
}

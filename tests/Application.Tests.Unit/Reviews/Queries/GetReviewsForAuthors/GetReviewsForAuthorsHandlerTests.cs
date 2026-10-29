namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.GetReviewsForAuthors;

using System.Threading;
using System.Threading.Tasks;
using Application.Authors.Entities;
using Application.Reviews;
using Application.Reviews.Entities;
using Application.Reviews.Queries.GetReviewsForAuthors;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetReviewsForAuthorsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturn_ReviewsGroupedByAuthor()
    {
        // Arrange
        var reviewer = Guid.NewGuid();
        var nonReviewer = Guid.NewGuid();
        var query = new GetReviewsForAuthorsQuery { AuthorIds = [reviewer, nonReviewer, reviewer] };

        var repository = Substitute.For<IReviewsRepository>();
        var handler = new GetReviewsForAuthorsHandler(repository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = repository.GetReviewsForAuthors(query.AuthorIds, token).Returns(
        [
            new Review(Guid.NewGuid(), 5, ReviewAuthor: new ReviewAuthor(reviewer, "First", "Last")),
            new Review(Guid.NewGuid(), 3, ReviewAuthor: new ReviewAuthor(reviewer, "First", "Last"))
        ]);

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await repository.Received(1).GetReviewsForAuthors(query.AuthorIds, token);

        result.Count.ShouldBe(2);
        result[reviewer].Select(r => r.Stars).ShouldBe([5, 3]);
        result[nonReviewer].ShouldBeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldSkip_ReviewsWithoutAuthor()
    {
        // Arrange
        var author = Guid.NewGuid();
        var query = new GetReviewsForAuthorsQuery { AuthorIds = [author] };

        var repository = Substitute.For<IReviewsRepository>();
        var handler = new GetReviewsForAuthorsHandler(repository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = repository.GetReviewsForAuthors(query.AuthorIds, token).Returns(
        [
            new Review(Guid.NewGuid(), 5, ReviewAuthor: new ReviewAuthor(author, "First", "Last")),
            new Review(Guid.NewGuid(), 1)
        ]);

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        result.Count.ShouldBe(1);
        result[author].Select(r => r.Stars).ShouldBe([5]);
    }
}

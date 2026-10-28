namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.GetReviews;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Paging;
using Application.Reviews;
using Application.Reviews.Entities;
using Application.Reviews.Queries.GetReviews;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetReviewsHandlerTests
{
    private static readonly Guid MovieId = Guid.NewGuid();
    private static readonly Guid AuthorId = Guid.NewGuid();

    [Fact]
    public async Task Handle_ShouldReturn_Page()
    {
        // Arrange
        var query = new GetReviewsQuery { MovieId = MovieId, AuthorId = AuthorId, Offset = 20, Limit = 5 };

        var repository = Substitute.For<IReviewsRepository>();
        var handler = new GetReviewsHandler(repository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = repository.GetReviews(MovieId, AuthorId, 20, 5, token).Returns(([new Review(Guid.Empty, 5)], 21));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await repository.Received(1).GetReviews(MovieId, AuthorId, 20, 5, token);

        _ = result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(1);
        result.Items[0].Stars.ShouldBe(5);
        result.Offset.ShouldBe(20);
        result.Limit.ShouldBe(5);
        result.TotalCount.ShouldBe(21);
    }

    [Fact]
    public async Task Handle_ShouldStartAt_FirstPage()
    {
        // Arrange
        var query = new GetReviewsQuery();

        var repository = Substitute.For<IReviewsRepository>();
        var handler = new GetReviewsHandler(repository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = repository.GetReviews(null, null, 0, Paging.DefaultLimit, token).Returns(([new Review(Guid.Empty, 5)], 1));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await repository.Received(1).GetReviews(null, null, 0, Paging.DefaultLimit, token);

        result.Offset.ShouldBe(0);
        result.Limit.ShouldBe(Paging.DefaultLimit);
        result.TotalCount.ShouldBe(1);
    }
}

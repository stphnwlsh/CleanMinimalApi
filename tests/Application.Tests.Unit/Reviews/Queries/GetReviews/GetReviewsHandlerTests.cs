namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.GetReviews;

using System.Threading;
using System.Threading.Tasks;
using Application.Reviews;
using CleanMinimalApi.Application.Entities;
using CleanMinimalApi.Application.Reviews.Queries.GetReviews;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetReviewsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetReviewsQuery();

        var context = Substitute.For<IReviewsRepository>();
        var handler = new GetReviewsHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.GetReviews(token).Returns(new List<Review> {
            new Review{
                Id = Guid.Empty,
                Stars = 5,
                ReviewAuthorId = Guid.Empty,
                ReviewedMovieId = Guid.Empty
            }
        });

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).GetReviews(token);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(Guid.Empty);
        result[0].Stars.ShouldBe(5);
        result[0].ReviewAuthorId.ShouldBe(Guid.Empty);
        result[0].ReviewedMovieId.ShouldBe(Guid.Empty);
    }
}
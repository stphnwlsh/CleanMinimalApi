namespace CleanMinimalApi.Application.Tests.Unit.Reviews.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Entities;
using CleanMinimalApi.Application.Reviews.ReadAll;
using NSubstitute;
using Shouldly;
using Xunit;

public class ReadAllHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadAllQuery();

        var context = Substitute.For<ReviewsRepository>();
        var handler = new ReadAllHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.ReadAllReviews(token).Returns(new List<Review> {
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
        _ = await context.Received(1).ReadAllReviews(token);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(Guid.Empty);
        result[0].Stars.ShouldBe(5);
        result[0].ReviewAuthorId.ShouldBe(Guid.Empty);
        result[0].ReviewedMovieId.ShouldBe(Guid.Empty);
    }
}

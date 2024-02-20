namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.GetReviewById;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Reviews;
using Application.Reviews.Entities;
using Application.Reviews.Queries.GetReviewById;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetReviewByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetReviewByIdQuery
        {
            Id = Guid.Empty
        };

        var context = Substitute.For<IReviewsRepository>();
        var handler = new GetReviewByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.GetReviewById(Arg.Any<Guid>(), token).Returns(new Review(Guid.Empty, 5));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).GetReviewById(query.Id, token);

        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<Review>();

        result.Id.ShouldBe(Guid.Empty);
        result.Stars.ShouldBe(5);
    }


    [Fact]
    public async Task Handle_ShouldThrowException_DoesNotExist()
    {
        // Arrange
        var query = new GetReviewByIdQuery
        {
            Id = Guid.Empty
        };

        var context = Substitute.For<IReviewsRepository>();
        var handler = new GetReviewByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(query, token));

        // Assert
        _ = await context.Received(1).GetReviewById(query.Id, token);

        exception.Message.ShouldBe("The Review with the supplied id was not found.");
    }
}

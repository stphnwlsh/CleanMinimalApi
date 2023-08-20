namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Commands.DeleteReview;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Reviews;
using Application.Reviews.Commands.DeleteReview;
using NSubstitute;
using Shouldly;
using Xunit;

public class DeleteReviewHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Command()
    {
        // Arrange
        var command = new DeleteReviewCommand { Id = Guid.Empty };
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(true);

        var handler = new DeleteReviewHandler(reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await reviewsRepository.Received(1).ReviewExists(command.Id, token);
        _ = await reviewsRepository.Received(1).DeleteReview(command.Id, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_ReviewDoesNotExist()
    {
        // Arrange
        var command = new DeleteReviewCommand { Id = Guid.Empty };
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(false);

        var handler = new DeleteReviewHandler(reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Review with the supplied id was not found.");

        _ = await reviewsRepository.Received(1).ReviewExists(command.Id, token);
        _ = await reviewsRepository.DidNotReceive().DeleteReview(command.Id, token);
    }
}

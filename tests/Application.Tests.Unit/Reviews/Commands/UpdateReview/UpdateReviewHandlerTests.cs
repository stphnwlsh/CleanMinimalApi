namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Commands.UpdateReview;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Reviews;
using Application.Reviews.Commands.UpdateReview;
using NSubstitute;
using Shouldly;
using Xunit;

public class UpdateReviewHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Command()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            Stars = 1
        };

        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(true);
        _ = reviewsRepository.UpdateReview(default, default, default).ReturnsForAnyArgs(true);

        var handler = new UpdateReviewHandler(reviewsRepository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await handler.Handle(command, token);

        // Assert
        result.ShouldBeTrue();

        _ = await reviewsRepository.Received(1).ReviewExists(command.Id, token);
        _ = await reviewsRepository.Received(1).UpdateReview(command.Id, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_ReviewDoesNotExist()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            Stars = 5
        };

        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(false);

        var handler = new UpdateReviewHandler(reviewsRepository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var exception = await Should.ThrowAsync<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Review with the supplied id was not found.");

        _ = await reviewsRepository.Received(1).ReviewExists(command.Id, token);
        _ = await reviewsRepository.DidNotReceiveWithAnyArgs().UpdateReview(default, default, default);
    }
}

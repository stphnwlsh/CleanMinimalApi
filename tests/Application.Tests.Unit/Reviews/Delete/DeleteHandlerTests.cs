namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Delete;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Reviews.Delete;
using NSubstitute;
using Shouldly;
using Xunit;

public class DeleteHandlerTests
{
    [Fact]
    public async Task HandleShouldPassThroughCommand()
    {
        // Arrange
        var command = new DeleteCommand { Id = Guid.Empty };
        var reviewsRepository = Substitute.For<ReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(true);

        var handler = new DeleteHandler(reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await reviewsRepository.Received(1).ReviewExists(command.Id, token);
        _ = await reviewsRepository.Received(1).DeleteReview(command.Id, token);
    }

    [Fact]
    public async Task HandleShouldThrowExceptionAuthorDoesNotExist()
    {
        // Arrange
        var command = new DeleteCommand { Id = Guid.Empty };
        var reviewsRepository = Substitute.For<ReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(false);

        var handler = new DeleteHandler(reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Review with the supplied id was not found.");

        _ = await reviewsRepository.Received(1).ReviewExists(command.Id, token);
        _ = await reviewsRepository.DidNotReceive().DeleteReview(command.Id, token);
    }
}

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
            Id = Guid.Empty,
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 1
        };
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(true);

        var handler = new UpdateReviewHandler(reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await reviewsRepository.Received(1).UpdateReview(command.Id, command.AuthorId, command.MovieId, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_ReviewDoesNotExist()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.Empty,
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(false);

        var handler = new UpdateReviewHandler(reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Review with the supplied id was not found.");

        _ = await reviewsRepository.DidNotReceive().UpdateReview(command.Id, command.AuthorId, command.MovieId, command.Stars, token);
    }
}

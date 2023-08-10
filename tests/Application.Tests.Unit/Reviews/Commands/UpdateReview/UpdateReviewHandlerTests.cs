namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Commands.UpdateReview;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Reviews;
using Application.Reviews.Commands.UpdateReview;
using CleanMinimalApi.Application.Authors;
using CleanMinimalApi.Application.Movies;
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

        var authorsRepository = Substitute.For<IAuthorsRepository>();
        var moviesRepository = Substitute.For<IMoviesRepository>();
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(true);
        _ = authorsRepository.AuthorExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.MovieExists(default, default).ReturnsForAnyArgs(true);

        var handler = new UpdateReviewHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await reviewsRepository.Received(1).ReviewExists(command.Id, token);
        _ = await authorsRepository.Received(1).AuthorExists(command.AuthorId, token);
        _ = await moviesRepository.Received(1).MovieExists(command.MovieId, token);
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

        var authorsRepository = Substitute.For<IAuthorsRepository>();
        var moviesRepository = Substitute.For<IMoviesRepository>();
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(false);
        _ = authorsRepository.AuthorExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.MovieExists(default, default).ReturnsForAnyArgs(true);

        var handler = new UpdateReviewHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Review with the supplied id was not found.");

        _ = await reviewsRepository.Received(1).ReviewExists(command.Id, token);
        _ = await authorsRepository.DidNotReceive().AuthorExists(command.AuthorId, token);
        _ = await moviesRepository.DidNotReceive().MovieExists(command.MovieId, token);
        _ = await reviewsRepository.DidNotReceive().UpdateReview(command.Id, command.AuthorId, command.MovieId, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_AuthorDoesNotExist()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.Empty,
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        var authorsRepository = Substitute.For<IAuthorsRepository>();
        var moviesRepository = Substitute.For<IMoviesRepository>();
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(true);
        _ = authorsRepository.AuthorExists(default, default).ReturnsForAnyArgs(false);
        _ = moviesRepository.MovieExists(default, default).ReturnsForAnyArgs(true);

        var handler = new UpdateReviewHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Author with the supplied id was not found.");

        _ = await reviewsRepository.Received(1).ReviewExists(command.Id, token);
        _ = await authorsRepository.Received(1).AuthorExists(command.AuthorId, token);
        _ = await moviesRepository.DidNotReceive().MovieExists(command.MovieId, token);
        _ = await reviewsRepository.DidNotReceive().CreateReview(command.AuthorId, command.MovieId, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_MovieDoesNotExist()
    {
        // Arrange
        var command = new UpdateReviewCommand
        {
            Id = Guid.Empty,
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        var authorsRepository = Substitute.For<IAuthorsRepository>();
        var moviesRepository = Substitute.For<IMoviesRepository>();
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = reviewsRepository.ReviewExists(default, default).ReturnsForAnyArgs(true);
        _ = authorsRepository.AuthorExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.MovieExists(default, default).ReturnsForAnyArgs(false);

        var handler = new UpdateReviewHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Movie with the supplied id was not found.");

        _ = await reviewsRepository.Received(1).ReviewExists(command.Id, token);
        _ = await authorsRepository.Received(1).AuthorExists(command.AuthorId, token);
        _ = await moviesRepository.Received(1).MovieExists(command.MovieId, token);
        _ = await reviewsRepository.DidNotReceive().CreateReview(command.AuthorId, command.MovieId, command.Stars, token);
    }
}

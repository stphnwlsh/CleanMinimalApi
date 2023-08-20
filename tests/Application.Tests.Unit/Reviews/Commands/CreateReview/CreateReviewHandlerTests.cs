namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Commands.CreateReview;

using System.Threading;
using System.Threading.Tasks;
using Application.Authors;
using Application.Common.Exceptions;
using Application.Movies;
using Application.Reviews;
using Application.Reviews.Commands.CreateReview;
using NSubstitute;
using Shouldly;
using Xunit;

public class CreateHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Command()
    {
        // Arrange
        var command = new CreateReviewCommand
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        var authorsRepository = Substitute.For<IAuthorsRepository>();
        var moviesRepository = Substitute.For<IMoviesRepository>();
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = authorsRepository.AuthorExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.MovieExists(default, default).ReturnsForAnyArgs(true);

        var handler = new CreateReviewHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await authorsRepository.Received(1).AuthorExists(command.AuthorId, token);
        _ = await moviesRepository.Received(1).MovieExists(command.MovieId, token);
        _ = await reviewsRepository.Received(1).CreateReview(command.AuthorId, command.MovieId, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_AuthorDoesNotExist()
    {
        // Arrange
        var command = new CreateReviewCommand
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        var authorsRepository = Substitute.For<IAuthorsRepository>();
        var moviesRepository = Substitute.For<IMoviesRepository>();
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = authorsRepository.AuthorExists(default, default).ReturnsForAnyArgs(false);
        _ = moviesRepository.MovieExists(default, default).ReturnsForAnyArgs(true);

        var handler = new CreateReviewHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Author with the supplied id was not found.");

        _ = await authorsRepository.Received(1).AuthorExists(command.AuthorId, token);
        _ = await moviesRepository.DidNotReceive().MovieExists(command.MovieId, token);
        _ = await reviewsRepository.DidNotReceive().CreateReview(command.AuthorId, command.MovieId, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_MovieDoesNotExist()
    {
        // Arrange
        var command = new CreateReviewCommand
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };
        var authorsRepository = Substitute.For<IAuthorsRepository>();
        var moviesRepository = Substitute.For<IMoviesRepository>();
        var reviewsRepository = Substitute.For<IReviewsRepository>();

        _ = authorsRepository.AuthorExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.MovieExists(default, default).ReturnsForAnyArgs(false);

        var handler = new CreateReviewHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Movie with the supplied id was not found.");

        _ = await authorsRepository.Received(1).AuthorExists(command.AuthorId, token);
        _ = await moviesRepository.Received(1).MovieExists(command.MovieId, token);
        _ = await reviewsRepository.DidNotReceive().CreateReview(command.AuthorId, command.MovieId, command.Stars, token);
    }
}

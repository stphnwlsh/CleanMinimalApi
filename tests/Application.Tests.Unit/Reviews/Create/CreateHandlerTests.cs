namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Create;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Reviews.Create;
using NSubstitute;
using Shouldly;
using Xunit;

public class CreateHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Command()
    {
        // Arrange
        var command = new CreateCommand
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };
        var authorsRepository = Substitute.For<AuthorsRepository>();
        var moviesRepository = Substitute.For<MoviesRepository>();
        var reviewsRepository = Substitute.For<ReviewsRepository>();

        _ = authorsRepository.AuthorExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.MovieExists(default, default).ReturnsForAnyArgs(true);

        var handler = new CreateHandler(authorsRepository, moviesRepository, reviewsRepository);
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
        var command = new CreateCommand
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };
        var authorsRepository = Substitute.For<AuthorsRepository>();
        var moviesRepository = Substitute.For<MoviesRepository>();
        var reviewsRepository = Substitute.For<ReviewsRepository>();

        _ = authorsRepository.AuthorExists(default, default).ReturnsForAnyArgs(false);

        var handler = new CreateHandler(authorsRepository, moviesRepository, reviewsRepository);
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
        var command = new CreateCommand
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };
        var authorsRepository = Substitute.For<AuthorsRepository>();
        var moviesRepository = Substitute.For<MoviesRepository>();
        var reviewsRepository = Substitute.For<ReviewsRepository>();

        _ = authorsRepository.AuthorExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.MovieExists(default, default).ReturnsForAnyArgs(false);

        var handler = new CreateHandler(authorsRepository, moviesRepository, reviewsRepository);
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

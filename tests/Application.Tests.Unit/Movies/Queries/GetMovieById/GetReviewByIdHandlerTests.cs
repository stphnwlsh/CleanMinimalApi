namespace CleanMinimalApi.Application.Tests.Unit.Movies.Queries.GetMovieById;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Movies;
using Application.Movies.Entities;
using Application.Movies.Queries.GetMovieById;
using NSubstitute;
using Shouldly;
using Xunit;

public class ReadByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetMovieByIdQuery
        {
            Id = Guid.Empty
        };

        var context = Substitute.For<IMoviesRepository>();
        var handler = new GetMovieByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.GetMovieById(Arg.Any<Guid>(), token).Returns(new Movie(Guid.Empty, "Title"));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).GetMovieById(query.Id, token);

        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<Movie>();

        result.Id.ShouldBe(Guid.Empty);
        result.Title.ShouldBe("Title");
        result.Reviews.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_DoesNotExist()
    {
        // Arrange
        var query = new GetMovieByIdQuery
        {
            Id = Guid.Empty
        };

        var context = Substitute.For<IMoviesRepository>();
        var handler = new GetMovieByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(query, token));

        // Assert
        _ = await context.Received(1).GetMovieById(query.Id, token);

        exception.Message.ShouldBe("The Movie with the supplied id was not found.");
    }
}

namespace CleanMinimalApi.Application.Tests.Unit.Movies.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Movies.ReadById;
using CleanMinimalApi.Domain.Movies.Entities;
using NSubstitute;
using Xunit;

public class ReadByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<IMoviesRepository>();
        var handler = new ReadByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.ReadMovieById(Arg.Any<Guid>(), token).Returns(new Movie
        {
            Id = Guid.Empty,
            Title = "Title"
        });

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadMovieById(query.Id, token);
    }
}

namespace CleanMinimalApi.Application.Tests.Unit.Reviews.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Reviews.ReadById;
using CleanMinimalApi.Domain.Reviews.Entities;
using NSubstitute;
using Xunit;

public class ReadByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<IReviewsRepository>();
        var handler = new ReadByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.ReadReviewById(Arg.Any<Guid>(), token).Returns(new Review
        {
            Id = Guid.Empty,
            Stars = 5,
            ReviewAuthorId = Guid.Empty,
            ReviewedMovieId = Guid.Empty
        });

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadReviewById(query.Id, token);
    }
}

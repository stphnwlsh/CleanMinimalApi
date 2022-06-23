namespace CleanMinimalApi.Application.Tests.Unit.Reviews.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Common.Interfaces;
using Entities;
using CleanMinimalApi.Application.Reviews.ReadById;
using NSubstitute;
using Shouldly;
using Xunit;

public class ReadByIdHandlerTests
{
    [Fact]
    public async Task HandleShouldPassThroughQuery()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<ReviewsRepository>();
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
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadReviewById(query.Id, token);

        _ = result.ShouldNotBeNull();
        result.Id.ShouldBe(Guid.Empty);
        result.Stars.ShouldBe(5);
        result.ReviewAuthorId.ShouldBe(Guid.Empty);
        result.ReviewedMovieId.ShouldBe(Guid.Empty);
    }


    [Fact]
    public async Task HandleShouldThrowExceptionDoesNotExist()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<ReviewsRepository>();
        var handler = new ReadByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(query, token));

        // Assert
        _ = await context.Received(1).ReadReviewById(query.Id, token);

        exception.Message.ShouldBe("The Review with the supplied id was not found.");
    }
}

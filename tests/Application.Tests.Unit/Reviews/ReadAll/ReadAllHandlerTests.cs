namespace CleanMinimalApi.Application.Tests.Unit.Reviews.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Reviews.ReadAll;
using NSubstitute;
using Xunit;

public class ReadAllHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadAllQuery();

        var context = Substitute.For<IReviewsRepository>();
        var handler = new ReadAllHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadAllReviews(token);
    }
}

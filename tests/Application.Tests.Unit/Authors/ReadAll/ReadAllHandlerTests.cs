namespace CleanMinimalApi.Application.Tests.Unit.Authors.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors.ReadAll;
using CleanMinimalApi.Application.Common.Interfaces;
using NSubstitute;
using Xunit;

public class ReadAllHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadAllQuery();

        var context = Substitute.For<IAuthorsRepository>();
        var handler = new ReadAllHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadAllAuthors(token);
    }
}

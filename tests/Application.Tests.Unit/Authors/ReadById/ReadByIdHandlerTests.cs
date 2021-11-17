namespace CleanMinimalApi.Application.Tests.Unit.Authors.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors.ReadById;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Domain.Authors.Entities;
using NSubstitute;
using Xunit;

public class ReadByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<IAuthorsRepository>();
        var handler = new ReadByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.ReadAuthorById(Arg.Any<Guid>(), token).Returns(new Author
        {
            Id = Guid.Empty,
            FirstName = "FirstName",
            LastName = "LastName"
        });

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadAuthorById(query.Id, token);
    }
}

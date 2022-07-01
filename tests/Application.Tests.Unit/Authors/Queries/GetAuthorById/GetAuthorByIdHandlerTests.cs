namespace CleanMinimalApi.Application.Tests.Unit.Authors.Queries.GetAuthorById;

using System.Threading;
using System.Threading.Tasks;
using Application.Authors;
using Application.Authors.Entities;
using Application.Authors.Queries.GetAuthorById;
using Application.Common.Exceptions;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetAuthorByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetAuthorByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<IAuthorsRepository>();
        var handler = new GetAuthorByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.GetAuthorById(Arg.Any<Guid>(), token).Returns(new Author
        {
            Id = Guid.Empty,
            FirstName = "FirstName",
            LastName = "LastName"
        });

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).GetAuthorById(query.Id, token);

        _ = result.ShouldNotBeNull();
        result.Id.ShouldBe(Guid.Empty);
        result.FirstName.ShouldBe("FirstName");
        result.LastName.ShouldBe("LastName");
    }


    [Fact]
    public async Task Handle_ShouldThrowException_DoesNotExist()
    {
        // Arrange
        var query = new GetAuthorByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<IAuthorsRepository>();
        var handler = new GetAuthorByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(query, token));

        // Assert
        _ = await context.Received(1).GetAuthorById(query.Id, token);

        exception.Message.ShouldBe("The Author with the supplied id was not found.");
    }
}

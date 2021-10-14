namespace CleanMinimalApi.Application.Tests.Unit.Notes.List;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Notes.List;
using NSubstitute;
using Xunit;

[ExcludeFromCodeCoverage]
public class ListNotesHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var command = new ListNotesQuery();

        var context = Substitute.For<INotesContext>();
        var handler = new ListNotesHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await context.Received(1).List(token);
    }
}

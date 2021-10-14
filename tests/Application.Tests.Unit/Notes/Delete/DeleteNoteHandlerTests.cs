namespace CleanMinimalApi.Application.Tests.Unit.Notes.Delete;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Notes.Delete;
using NSubstitute;
using Xunit;

[ExcludeFromCodeCoverage]
public class DeleteNoteHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Command()
    {
        // Arrange
        var command = new DeleteNoteCommand(42);

        var context = Substitute.For<INotesContext>();
        var handler = new DeleteNoteHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await context.Received(1).Delete(command.Id, token);
    }
}

namespace CleanMinimalApi.Application.Tests.Unit.Notes.Update;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Notes.Update;
using NSubstitute;
using Xunit;

[ExcludeFromCodeCoverage]
public class UpdateNoteHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Command()
    {
        // Arrange
        var command = new UpdateNoteCommand(42, "Meaning of Life");

        var context = Substitute.For<INotesContext>();
        var handler = new UpdateNoteHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await context.Received(1).Update(command.Id, command.Text, token);
    }
}

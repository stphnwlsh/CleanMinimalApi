namespace CleanMinimalApi.Application.Tests.Unit.Notes.Create;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Notes.Create;
using NSubstitute;
using Xunit;

[ExcludeFromCodeCoverage]
public class CreateNoteHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Command()
    {
        // Arrange
        var command = new CreateNoteCommand("Meaning of Life");

        var context = Substitute.For<INotesContext>();
        var handler = new CreateNoteHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await context.Received(1).Create(command.Text, token);
    }
}

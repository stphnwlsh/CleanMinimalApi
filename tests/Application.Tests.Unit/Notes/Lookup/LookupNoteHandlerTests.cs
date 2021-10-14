namespace CleanMinimalApi.Application.Tests.Unit.Notes.Lookup;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Notes.Lookup;
using NSubstitute;
using Xunit;

[ExcludeFromCodeCoverage]
public class LookupNoteHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var command = new LookupNoteQuery(42);

        var context = Substitute.For<INotesContext>();
        var handler = new LookupNoteHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await context.Received(1).Lookup(command.Id, token);
    }
}

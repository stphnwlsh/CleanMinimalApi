namespace CleanMinimalApi.Application.Tests.Unit.Version.ReadVersion;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Versions.ReadVersion;
using Shouldly;
using Xunit;

public class ReadVersionHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadVersionQuery();

        var handler = new ReadVersionHandler();
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = result.ShouldNotBeNull();

        _ = result.FileVersion.ShouldBeOfType<string>();
        result.FileVersion.ShouldNotBeNullOrWhiteSpace();
        _ = result.InformationalVersion.ShouldBeOfType<string>();
        result.InformationalVersion.ShouldNotBeNullOrWhiteSpace();
    }
}

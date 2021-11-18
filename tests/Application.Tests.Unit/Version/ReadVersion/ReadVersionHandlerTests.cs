namespace CleanMinimalApi.Application.Tests.Unit.Version.ReadVersion;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Versions.ReadVersion;
using NSubstitute;
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
        result.FileVersion.ShouldBe("15.0.0");
        result.InformationalVersion.ShouldBe("17.0.0");
    }
}

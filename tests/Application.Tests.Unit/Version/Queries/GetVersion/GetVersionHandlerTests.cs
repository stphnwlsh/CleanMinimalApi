namespace CleanMinimalApi.Application.Tests.Unit.Version.Queries.GetVersion;

using System.Threading;
using System.Threading.Tasks;
using Application.Versions.Entities;
using Application.Versions.Queries.GetVersion;
using Shouldly;
using Xunit;

public class GetVersionHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetVersionQuery();

        var handler = new GetVersionHandler();
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<Version>();

        _ = result.FileVersion.ShouldBeOfType<string>();
        result.FileVersion.ShouldNotBeNullOrWhiteSpace();
        _ = result.InformationalVersion.ShouldBeOfType<string>();
        result.InformationalVersion.ShouldNotBeNullOrWhiteSpace();
    }
}

namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Threading.Tasks;
using Shouldly;
using Xunit;
using NSubstitute;
using MediatR;
using Presentation.Endpoints;
using Entities = Application.Versions.Entities;
using Queries = Application.Versions.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

public class VersionEndpointTests
{
    [Fact]
    public async Task GetVersion_ShouldReturn_Ok()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator.Send(Arg.Any<Queries.GetVersion.GetVersionQuery>()).ReturnsForAnyArgs(new Entities.Version
        {
            FileVersion = "1.2.3.4",
            InformationalVersion = "5.6.7.8"
        });

        // Act
        var response = await VersionEndpoints.GetVersion(mediator);

        // Assert
        var result = response.ShouldBeOfType<Ok<Entities.Version>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<Entities.Version>();

        _ = value.FileVersion.ShouldBeOfType<string>();
        value.FileVersion.ShouldBe("1.2.3.4");
        _ = value.InformationalVersion.ShouldBeOfType<string>();
        value.InformationalVersion.ShouldBe("5.6.7.8");
    }
}

namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Net;
using System.Threading.Tasks;
using Application.Versions.Entities;
using Extensions;
using Shouldly;
using Xunit;
using NSubstitute;
using MediatR;
using Presentation.Endpoints;
using Entities = Application.Versions.Entities;
using Queries = Application.Versions.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class VersionEndpointTests
{
    [Fact]
    public async Task GetVersion_ShouldReturn_Ok()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator.Send(Arg.Any<Queries.GetVersion.GetVersionQuery>()).ReturnsForAnyArgs(new Entities.Version
        {
            FileVersion = string.Empty,
            InformationalVersion = string.Empty
        });

        // Act
        var response = await VersionEndpoints.GetVersion(mediator);

        // Assert
        var objectResponse = Assert.IsType<ObjectResult>(response);

        objectResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = response.ShouldNotBeNull();

        //_ = response.FileVersion.ShouldBeOfType<string>();
        //response.FileVersion.ShouldNotBeNullOrWhiteSpace();
        //_ = response.InformationalVersion.ShouldBeOfType<string>();
        //response.InformationalVersion.ShouldNotBeNullOrWhiteSpace();
    }
}

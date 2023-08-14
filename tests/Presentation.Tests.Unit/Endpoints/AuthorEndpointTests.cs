namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Presentation.Endpoints;
using Shouldly;
using Xunit;
using Entities = Application.Authors.Entities;
using Queries = Application.Authors.Queries;

public class AuthorEndpointTests
{
    [Fact]
    public async Task GetAuthors_ShouldReturn_Ok()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator.Send(Arg.Any<Queries.GetAuthors.GetAuthorsQuery>()).ReturnsForAnyArgs(new List<Entities.Author>
        {
            new Entities.Author
            {
                Id = Guid.Empty,
                FirstName = "Lorem",
                LastName = "Ipsum"
            }
        });

        // Act
        var response = await AuthorsEndpoints.GetAuthors(mediator);

        // Assert
        var result = response.ShouldBeOfType<Ok<List<Entities.Author>>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<List<Entities.Author>>();

        _ = value[0].Id.ShouldBeOfType<Guid>();
        value[0].Id.ShouldBe(Guid.Empty);
        _ = value[0].FirstName.ShouldBeOfType<string>();
        value[0].FirstName.ShouldBe("Lorem");
        _ = value[0].LastName.ShouldBeOfType<string>();
        value[0].LastName.ShouldBe("Ipsum");
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_Ok()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        _ = mediator.Send(Arg.Any<Queries.GetAuthorById.GetAuthorByIdQuery>()).ReturnsForAnyArgs(new Entities.Author
        {
            Id = Guid.Empty,
            FirstName = "Lorem",
            LastName = "Ipsum"
        });

        // Act
        var response = await AuthorsEndpoints.GetAuthorById(Guid.Empty, mediator);

        // Assert
        var result = response.ShouldBeOfType<Ok<Entities.Author>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<Entities.Author>();

        _ = value.Id.ShouldBeOfType<Guid>();
        value.Id.ShouldBe(Guid.Empty);
        _ = value.FirstName.ShouldBeOfType<string>();
        value.FirstName.ShouldBe("Lorem");
        _ = value.LastName.ShouldBeOfType<string>();
        value.LastName.ShouldBe("Ipsum");
    }
}

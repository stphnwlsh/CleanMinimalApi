namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Reviews.Entities;
using CleanMinimalApi.Presentation.Endpoints;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
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
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetAuthors.GetAuthorsQuery>())
            .ReturnsForAnyArgs(
            [
                new Entities.Author(Guid.Empty, "Lorem", "Ipsum")
            ]);

        // Act
        var response = await AuthorEndpoints.GetAuthors(sender);

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
        _ = value[0].Reviews.ShouldBeAssignableTo<ICollection<Review>>();
        value[0].Reviews.ShouldBeNull();
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetAuthorById.GetAuthorByIdQuery>())
            .ReturnsForAnyArgs(new Entities.Author(Guid.Empty, "Lorem", "Ipsum"));

        // Act
        var response = await AuthorEndpoints.GetAuthorById(Guid.Empty, sender);

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
        _ = value.Reviews.ShouldBeAssignableTo<ICollection<Review>>();
        value.Reviews.ShouldBeNull();
    }
}

namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Paging;
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
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = sender
            .Send(Arg.Is<Queries.GetAuthors.GetAuthorsQuery>(q => q.Query == "Lorem" && q.Offset == 20 && q.Limit == 5), token)
            .Returns(new Page<Entities.AuthorSummary>([new Entities.AuthorSummary(Guid.Empty, "Lorem", "Ipsum", 2, 3.5)], 20, 5, 21));

        // Act
        var response = await AuthorEndpoints.GetAuthors("Lorem", sender, token, 20, 5);

        // Assert
        var result = response.ShouldBeOfType<Ok<Page<Entities.AuthorSummary>>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldNotBeNull();

        value.Items.Count.ShouldBe(1);
        value.Items[0].Id.ShouldBe(Guid.Empty);
        value.Items[0].FirstName.ShouldBe("Lorem");
        value.Items[0].LastName.ShouldBe("Ipsum");
        value.Items[0].ReviewCount.ShouldBe(2);
        value.Items[0].AverageStars.ShouldBe(3.5);
        value.Offset.ShouldBe(20);
        value.Limit.ShouldBe(5);
        value.TotalCount.ShouldBe(21);
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var id = Guid.NewGuid();
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = sender
            .Send(Arg.Is<Queries.GetAuthorById.GetAuthorByIdQuery>(q => q.Id == id), token)
            .Returns(new Entities.Author(id, "Lorem", "Ipsum", []));

        // Act
        var response = await AuthorEndpoints.GetAuthorById(id, sender, token);

        // Assert
        var result = response.ShouldBeOfType<Ok<Entities.Author>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldNotBeNull();

        value.Id.ShouldBe(id);
        value.FirstName.ShouldBe("Lorem");
        value.LastName.ShouldBe("Ipsum");
        value.Reviews.ShouldBeEmpty();
    }
}

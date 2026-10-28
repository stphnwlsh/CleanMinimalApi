namespace CleanMinimalApi.Application.Tests.Unit.Authors.Queries.SearchAuthors;

using System.Threading;
using System.Threading.Tasks;
using Application.Authors;
using Application.Authors.Entities;
using Application.Authors.Queries.SearchAuthors;
using Application.Common.Paging;
using NSubstitute;
using Shouldly;
using Xunit;

public class SearchAuthorsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new SearchAuthorsQuery { Query = "Lorem", Limit = 1, Cursor = Paging.Encode(2) };

        var repository = Substitute.For<IAuthorsRepository>();
        var handler = new SearchAuthorsHandler(repository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = repository.SearchAuthors("Lorem", 2, 1, token).Returns(([new Author(Guid.Empty, "First", "Last")], 5));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await repository.Received(1).SearchAuthors("Lorem", 2, 1, token);

        _ = result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(1);
        result.Items[0].FirstName.ShouldBe("First");
        result.TotalCount.ShouldBe(5);
        result.NextCursor.ShouldBe(Paging.Encode(3));
    }

    [Fact]
    public async Task Handle_ShouldStartAt_FirstPage()
    {
        // Arrange
        var query = new SearchAuthorsQuery();

        var repository = Substitute.For<IAuthorsRepository>();
        var handler = new SearchAuthorsHandler(repository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = repository.SearchAuthors(null, 0, Paging.DefaultLimit, token).Returns(([new Author(Guid.Empty, "First", "Last")], 1));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await repository.Received(1).SearchAuthors(null, 0, Paging.DefaultLimit, token);

        result.TotalCount.ShouldBe(1);
        result.NextCursor.ShouldBeNull();
    }
}

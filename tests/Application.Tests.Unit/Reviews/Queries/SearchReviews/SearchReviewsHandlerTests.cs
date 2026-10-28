namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.SearchReviews;

using System.Threading;
using System.Threading.Tasks;
using Application.Authors.Entities;
using Application.Common.Paging;
using Application.Movies.Entities;
using Application.Reviews;
using Application.Reviews.Entities;
using Application.Reviews.Queries.SearchReviews;
using NSubstitute;
using Shouldly;
using Xunit;

public class SearchReviewsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var query = new SearchReviewsQuery { MovieId = movieId, AuthorId = authorId, Limit = 1, Cursor = Paging.Encode(1) };

        var repository = Substitute.For<IReviewsRepository>();
        var handler = new SearchReviewsHandler(repository);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        _ = repository.SearchReviews(movieId, authorId, 1, 1, token)
            .Returns(([new Review(Guid.Empty, 5, new ReviewedMovie(movieId, "Title"), new ReviewAuthor(authorId, "First", "Last"))], 2));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await repository.Received(1).SearchReviews(movieId, authorId, 1, 1, token);

        _ = result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(1);
        result.Items[0].Stars.ShouldBe(5);
        result.TotalCount.ShouldBe(2);
        result.NextCursor.ShouldBeNull();
    }
}

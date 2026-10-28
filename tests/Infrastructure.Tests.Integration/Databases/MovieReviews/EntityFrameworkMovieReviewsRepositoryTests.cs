namespace CleanMinimalApi.Infrastructure.Tests.Integration.Databases.MovieReviews;

using Application.Reviews.Commands.CreateReview;
using Application.Reviews.Commands.UpdateReview;
using Shouldly;
using Xunit;

[Collection("MovieReviews")]
public class EntityFrameworkMovieReviewsRepositoryTests(MovieReviewsDataFixture fixture)
{

    #region Authors


    [Fact]
    public async Task GetAuthorById_ShouldReturn_Author()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var author = fixture.Context.Authors.First(a => a.FirstName == "One");

        // Act
        var result = await repository.GetAuthorById(author.Id, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.FirstName.ShouldBe("One");
        result.LastName.ShouldBe("One");
        result.Reviews.ShouldNotBeEmpty();
        result.Reviews.Count.ShouldBe(3);
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_Null()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await repository.GetAuthorById(Guid.Empty, token);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task AuthorExists_ShouldReturn_True()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var author = fixture.Context.Authors.First(a => a.FirstName == "One");

        // Act
        var result = await repository.AuthorExists(author.Id, token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task AuthorExists_ShouldReturn_False()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await repository.AuthorExists(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData(null, 3)]
    [InlineData("one", 1)]
    [InlineData("TWO TWO", 1)]
    [InlineData("missing", 0)]
    [InlineData("%", 0)]
    public async Task GetAuthors_ShouldReturn_Matches(string? query, int expected)
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var (items, totalCount) = await repository.GetAuthors(query, 0, 10, token);

        // Assert
        items.Count.ShouldBe(expected);
        totalCount.ShouldBe(expected);
        items.ShouldAllBe(a => a.Reviews != null);
    }

    [Fact]
    public async Task GetAuthors_ShouldReturn_Page()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var (items, totalCount) = await repository.GetAuthors(null, 1, 1, token);

        // Assert
        items.Count.ShouldBe(1);
        items[0].LastName.ShouldBe("Three");
        totalCount.ShouldBe(3);
    }

    #endregion Authors

    #region Movies


    [Fact]
    public async Task GetMovieById_ShouldReturn_Movie()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var movie = fixture.Context.Movies.First(m => m.Title == "One");

        // Act
        var result = await repository.GetMovieById(movie.Id, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Title.ShouldBe("One");
        result.Reviews.ShouldNotBeEmpty();
        result.Reviews.Count.ShouldBe(3);
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_Null()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await repository.GetMovieById(Guid.Empty, token);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task MovieExists_ShouldReturn_True()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var movie = fixture.Context.Movies.First(m => m.Title == "One");

        // Act
        var result = await repository.MovieExists(movie.Id, token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task MovieExists_ShouldReturn_False()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await repository.MovieExists(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData(null, 3)]
    [InlineData("t", 2)]
    [InlineData("ONE", 1)]
    [InlineData("missing", 0)]
    public async Task GetMovies_ShouldReturn_Matches(string? query, int expected)
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var (items, totalCount) = await repository.GetMovies(query, 0, 10, token);

        // Assert
        items.Count.ShouldBe(expected);
        totalCount.ShouldBe(expected);
        items.ShouldAllBe(m => m.Reviews != null);
    }

    [Fact]
    public async Task GetMovies_ShouldReturn_PagesInTitleOrder()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var (first, firstTotal) = await repository.GetMovies(null, 0, 2, token);
        var (second, secondTotal) = await repository.GetMovies(null, 2, 2, token);

        // Assert
        first.Select(m => m.Title).ShouldBe(["One", "Three"]);
        second.Select(m => m.Title).ShouldBe(["Two"]);
        firstTotal.ShouldBe(3);
        secondTotal.ShouldBe(3);
    }

    #endregion Movies

    #region Reviews

    [Fact]
    public async Task CreateReview_ShouldReturn_NewReviews()
    {
        // Arrange
        var review = new CreateReviewCommand
        {
            AuthorId = fixture.Context.Authors.First(a => a.FirstName == "One").Id,
            MovieId = fixture.Context.Movies.First(m => m.Title == "One").Id,
            Stars = 5
        };
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await fixture.Repository.CreateReview(review.AuthorId, review.MovieId, review.Stars, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.ReviewAuthor.ShouldNotBeNull().Id.ShouldBe(review.AuthorId);
        result.ReviewedMovie.ShouldNotBeNull().Id.ShouldBe(review.MovieId);
        result.Stars.ShouldBe(review.Stars);

        // Cleanup
        _ = await fixture.Repository.DeleteReview(result.Id, token);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_True()
    {
        // Arrange
        var deleted = fixture.Context.Reviews.First(r => r.Stars == 1);
        var (id, authorId, movieId) = (deleted.Id, deleted.ReviewAuthorId, deleted.ReviewedMovieId);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await fixture.Repository.DeleteReview(id, token);

        // Assert
        result.ShouldBeTrue();

        // Cleanup
        _ = await fixture.Repository.CreateReview(authorId, movieId, 1, token);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_False()
    {
        // Arrange
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await fixture.Repository.DeleteReview(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }


    [Fact]
    public async Task GetReviewById_ShouldReturn_Review()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var review = fixture.Context.Reviews.First(m => m.Stars == 5);

        // Act
        var result = await repository.GetReviewById(review.Id, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Stars.ShouldBe(5);
        _ = result.ReviewAuthor.ShouldNotBeNull();
        result.ReviewAuthor.Id.ShouldNotBe(Guid.Empty);
        result.ReviewAuthor.FirstName.ShouldBe("Two");
        result.ReviewAuthor.LastName.ShouldBe("Two");
        _ = result.ReviewedMovie.ShouldNotBeNull();
        result.ReviewedMovie.Id.ShouldNotBe(Guid.Empty);
        result.ReviewedMovie.Title.ShouldBe("Two");
    }

    [Fact]
    public async Task GetReviewById_ShouldReturn_Null()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await repository.GetReviewById(Guid.Empty, token);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task ReviewExists_ShouldReturn_True()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var review = fixture.Context.Reviews.First(m => m.Stars == 5);

        // Act
        var result = await repository.ReviewExists(review.Id, token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task ReviewExists_ShouldReturn_False()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await repository.ReviewExists(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_True()
    {
        // Arrange
        var original = fixture.Context.Reviews.First(a => a.Stars == 2);
        var review = new UpdateReviewCommand
        {
            Id = original.Id,
            Stars = 4
        };
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await fixture.Repository.UpdateReview(review.Id, review.Stars, token);

        // Assert
        result.ShouldBeTrue();

        var updatedReview = fixture.Context.Reviews.First(a => a.Id == review.Id);

        updatedReview.Id.ShouldBe(review.Id);
        updatedReview.ReviewAuthorId.ShouldBe(original.ReviewAuthorId);
        updatedReview.ReviewedMovieId.ShouldBe(original.ReviewedMovieId);
        updatedReview.Stars.ShouldBe(review.Stars);
        updatedReview.DateModified.ShouldBe(fixture.TimeProvider.GetUtcNow().UtcDateTime);

        // Cleanup
        _ = await fixture.Repository.UpdateReview(original.Id, original.Stars, token);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_False()
    {
        // Arrange
        var review = new UpdateReviewCommand
        {
            Id = Guid.Empty,
            Stars = 5
        };
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await fixture.Repository.UpdateReview(review.Id, review.Stars, token);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_ReviewsForMovie()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var movieId = fixture.Context.Movies.First(m => m.Title == "Two").Id;

        // Act
        var (items, totalCount) = await repository.GetReviews(movieId, null, 0, 50, token);

        // Assert
        items.ShouldNotBeEmpty();
        items.Count.ShouldBe(totalCount);
        items.ShouldAllBe(r => r.ReviewedMovie!.Id == movieId && r.ReviewedMovie.Title == "Two");
        items.ShouldAllBe(r => r.ReviewAuthor != null);
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_ReviewsForAuthorAndMovie()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var movieId = fixture.Context.Movies.First(m => m.Title == "Three").Id;
        var authorId = fixture.Context.Authors.First(a => a.FirstName == "Two").Id;

        // Act
        var (items, _) = await repository.GetReviews(movieId, authorId, 0, 50, token);

        // Assert
        items.ShouldAllBe(r => r.ReviewedMovie!.Id == movieId && r.ReviewAuthor!.Id == authorId);
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_Page()
    {
        // Arrange
        var repository = fixture.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var (items, totalCount) = await repository.GetReviews(null, null, 0, 2, token);

        // Assert
        items.Count.ShouldBe(2);
        totalCount.ShouldBeGreaterThan(2);
    }

    #endregion Reviews
}

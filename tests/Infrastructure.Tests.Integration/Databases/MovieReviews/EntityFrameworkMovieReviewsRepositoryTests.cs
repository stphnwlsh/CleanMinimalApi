namespace CleanMinimalApi.Infrastructure.Tests.Integration.Databases.MovieReviews;

using Application.Reviews.Commands.CreateReview;
using Application.Reviews.Commands.UpdateReview;
using Shouldly;
using Xunit;

public sealed class EntityFrameworkMovieReviewsRepositoryTests : IDisposable
{
    private readonly MovieReviewsTestDatabase database = new();

    public void Dispose()
    {
        this.database.Dispose();
    }

    #region Authors

    [Fact]
    public async Task GetAuthorById_ShouldReturn_Author()
    {
        // Arrange
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var author = this.database.Context.Authors.First(a => a.FirstName == "One");

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
        var repository = this.database.Repository;
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
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var author = this.database.Context.Authors.First(a => a.FirstName == "One");

        // Act
        var result = await repository.AuthorExists(author.Id, token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task AuthorExists_ShouldReturn_False()
    {
        // Arrange
        var repository = this.database.Repository;
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
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var (items, totalCount) = await repository.GetAuthors(query, 0, 10, token);

        // Assert
        items.Count.ShouldBe(expected);
        totalCount.ShouldBe(expected);
    }

    [Fact]
    public async Task GetAuthors_ShouldReturn_ReviewCountAndAverageStars()
    {
        // Arrange
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var (items, _) = await repository.GetAuthors("one", 0, 10, token);

        // Assert
        var author = items.ShouldHaveSingleItem();
        author.ReviewCount.ShouldBe(3);
        author.AverageStars.ShouldBe(2.0);
    }

    [Fact]
    public async Task GetAuthors_ShouldReturn_Page()
    {
        // Arrange
        var repository = this.database.Repository;
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
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var movie = this.database.Context.Movies.First(m => m.Title == "One");

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
        var repository = this.database.Repository;
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
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var movie = this.database.Context.Movies.First(m => m.Title == "One");

        // Act
        var result = await repository.MovieExists(movie.Id, token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task MovieExists_ShouldReturn_False()
    {
        // Arrange
        var repository = this.database.Repository;
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
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var (items, totalCount) = await repository.GetMovies(query, 0, 10, token);

        // Assert
        items.Count.ShouldBe(expected);
        totalCount.ShouldBe(expected);
    }

    [Fact]
    public async Task GetMovies_ShouldReturn_ReviewCountAndAverageStars()
    {
        // Arrange
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var (items, _) = await repository.GetMovies("one", 0, 10, token);

        // Assert
        var movie = items.ShouldHaveSingleItem();
        movie.ReviewCount.ShouldBe(3);
        movie.AverageStars.ShouldBe(2.7);
    }

    [Fact]
    public async Task GetMovies_ShouldReturn_NullAverageStars_WhenNoReviews()
    {
        // Arrange
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        this.database.Context.Reviews.RemoveRange(this.database.Context.Reviews);
        _ = await this.database.Context.SaveChangesAsync(token);

        // Act
        var (items, _) = await repository.GetMovies(null, 0, 10, token);

        // Assert
        items.ShouldAllBe(m => m.ReviewCount == 0 && m.AverageStars == null);
    }

    [Fact]
    public async Task GetMovies_ShouldReturn_PagesInTitleOrder()
    {
        // Arrange
        var repository = this.database.Repository;
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
            AuthorId = this.database.Context.Authors.First(a => a.FirstName == "One").Id,
            MovieId = this.database.Context.Movies.First(m => m.Title == "One").Id,
            Stars = 5
        };
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await this.database.Repository.CreateReview(review.AuthorId, review.MovieId, review.Stars, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.ReviewAuthor.ShouldNotBeNull().Id.ShouldBe(review.AuthorId);
        result.ReviewedMovie.ShouldNotBeNull().Id.ShouldBe(review.MovieId);
        result.Stars.ShouldBe(review.Stars);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_True()
    {
        // Arrange
        var id = this.database.Context.Reviews.First(r => r.Stars == 1).Id;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await this.database.Repository.DeleteReview(id, token);

        // Assert
        result.ShouldBeTrue();
        this.database.Context.Reviews.Any(r => r.Id == id).ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_False()
    {
        // Arrange
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await this.database.Repository.DeleteReview(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }


    [Fact]
    public async Task GetReviewById_ShouldReturn_Review()
    {
        // Arrange
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var review = this.database.Context.Reviews.First(m => m.Stars == 5);

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
        var repository = this.database.Repository;
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
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var review = this.database.Context.Reviews.First(m => m.Stars == 5);

        // Act
        var result = await repository.ReviewExists(review.Id, token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task ReviewExists_ShouldReturn_False()
    {
        // Arrange
        var repository = this.database.Repository;
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
        var original = this.database.Context.Reviews.First(a => a.Stars == 2);
        var review = new UpdateReviewCommand
        {
            Id = original.Id,
            Stars = 4
        };
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await this.database.Repository.UpdateReview(review.Id, review.Stars, token);

        // Assert
        result.ShouldBeTrue();

        var updatedReview = this.database.Context.Reviews.First(a => a.Id == review.Id);

        updatedReview.Id.ShouldBe(review.Id);
        updatedReview.ReviewAuthorId.ShouldBe(original.ReviewAuthorId);
        updatedReview.ReviewedMovieId.ShouldBe(original.ReviewedMovieId);
        updatedReview.Stars.ShouldBe(review.Stars);
        updatedReview.DateModified.ShouldBe(this.database.TimeProvider.GetUtcNow().UtcDateTime);
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
        var result = await this.database.Repository.UpdateReview(review.Id, review.Stars, token);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_ReviewsForMovie()
    {
        // Arrange
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var movieId = this.database.Context.Movies.First(m => m.Title == "Two").Id;

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
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var movieId = this.database.Context.Movies.First(m => m.Title == "Three").Id;
        var authorId = this.database.Context.Authors.First(a => a.FirstName == "Two").Id;

        // Act
        var (items, _) = await repository.GetReviews(movieId, authorId, 0, 50, token);

        // Assert
        items.ShouldAllBe(r => r.ReviewedMovie!.Id == movieId && r.ReviewAuthor!.Id == authorId);
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_Page()
    {
        // Arrange
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var (items, totalCount) = await repository.GetReviews(null, null, 0, 2, token);

        // Assert
        items.Count.ShouldBe(2);
        totalCount.ShouldBeGreaterThan(2);
    }

    [Fact]
    public async Task GetReviewsForMovies_ShouldReturn_ReviewsForEachMovie()
    {
        // Arrange
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var movieIds = this.database.Context.Movies.Where(m => m.Title != "Three").Select(m => m.Id).ToList();

        // Act
        var result = await repository.GetReviewsForMovies(movieIds, token);

        // Assert
        result.Count.ShouldBe(6);
        result.ShouldAllBe(r => movieIds.Contains(r.ReviewedMovie!.Id) && r.ReviewAuthor != null);
        result.Select(r => r.ReviewAuthor!.LastName).ShouldBeInOrder();
    }

    [Fact]
    public async Task GetReviewsForMovies_ShouldReturn_Empty_WhenNoMatches()
    {
        // Arrange
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Act
        var result = await repository.GetReviewsForMovies([Guid.NewGuid()], token);

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetReviewsForAuthors_ShouldReturn_ReviewsForEachAuthor()
    {
        // Arrange
        var repository = this.database.Repository;
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var authorIds = this.database.Context.Authors.Where(a => a.FirstName == "One").Select(a => a.Id).ToList();

        // Act
        var result = await repository.GetReviewsForAuthors(authorIds, token);

        // Assert
        result.Count.ShouldBe(3);
        result.ShouldAllBe(r => authorIds.Contains(r.ReviewAuthor!.Id) && r.ReviewedMovie != null);
        result.Select(r => r.ReviewedMovie!.Title).ShouldBe(["One", "Three", "Two"]);
    }

    #endregion Reviews
}

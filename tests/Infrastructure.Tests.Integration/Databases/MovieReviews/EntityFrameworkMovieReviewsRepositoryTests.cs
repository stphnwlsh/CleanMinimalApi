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
    public async Task GetAuthors_ShouldReturn_Authors()
    {
        // Arrange
        var repository = fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.GetAuthors(token);

        // Assert
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(3);
    }


    [Fact]
    public async Task GetAuthorById_ShouldReturn_Author()
    {
        // Arrange
        var repository = fixture.Repository;
        var token = new CancellationTokenSource().Token;
        var author = fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "One");

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
        var token = new CancellationTokenSource().Token;

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
        var token = new CancellationTokenSource().Token;
        var author = fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "One");

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
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.AuthorExists(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    #endregion Authors

    #region Movies

    [Fact]
    public async Task GetMovies_ShouldReturn_Movies()
    {
        // Arrange
        var repository = fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.GetMovies(token);

        // Assert
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(3);
    }


    [Fact]
    public async Task GetMovieById_ShouldReturn_Movie()
    {
        // Arrange
        var repository = fixture.Repository;
        var token = new CancellationTokenSource().Token;
        var movie = fixture.Context.Movies.FirstOrDefault(m => m.Title == "One");

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
        var token = new CancellationTokenSource().Token;

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
        var token = new CancellationTokenSource().Token;
        var movie = fixture.Context.Movies.FirstOrDefault(m => m.Title == "One");

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
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.MovieExists(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    #endregion Movies

    #region Reviews

    [Fact]
    public async Task CreateReview_ShouldReturn_NewReviews()
    {
        // Arrange
        var review = new CreateReviewCommand
        {
            AuthorId = fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "One").Id,
            MovieId = fixture.Context.Movies.FirstOrDefault(m => m.Title == "One").Id,
            Stars = 5
        };
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await fixture.Repository.CreateReview(review.AuthorId, review.MovieId, review.Stars, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.ReviewAuthor.Id.ShouldBe(review.AuthorId);
        result.ReviewedMovie.Id.ShouldBe(review.MovieId);
        result.Stars.ShouldBe(review.Stars);

        // Cleanup
        _ = await fixture.Repository.DeleteReview(result.Id, token);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_True()
    {
        // Arrange
        var id = fixture.Context.Reviews.FirstOrDefault(r => r.Stars == 1).Id;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await fixture.Repository.DeleteReview(id, token);

        // Assert
        result.ShouldBeTrue();

        // Cleanup
        var authorId = fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "Three").Id;
        var movieId = fixture.Context.Movies.FirstOrDefault(m => m.Title == "Three").Id;
        _ = await fixture.Repository.CreateReview(authorId, movieId, 1, token);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_False()
    {
        // Arrange
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await fixture.Repository.DeleteReview(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_Reviews()
    {
        // Arrange
        var repository = fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.GetReviews(token);

        // Assert
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(9);
    }


    [Fact]
    public async Task GetReviewById_ShouldReturn_Review()
    {
        // Arrange
        var repository = fixture.Repository;
        var token = new CancellationTokenSource().Token;
        var review = fixture.Context.Reviews.FirstOrDefault(m => m.Stars == 5);

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
        var token = new CancellationTokenSource().Token;

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
        var token = new CancellationTokenSource().Token;
        var review = fixture.Context.Reviews.FirstOrDefault(m => m.Stars == 5);

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
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.ReviewExists(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_True()
    {
        // Arrange
        var review = new UpdateReviewCommand
        {
            Id = fixture.Context.Reviews.FirstOrDefault(a => a.Stars == 2).Id,
            AuthorId = fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "Two").Id,
            MovieId = fixture.Context.Movies.FirstOrDefault(m => m.Title == "Two").Id,
            Stars = 4
        };
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await fixture.Repository.UpdateReview(review.Id, review.AuthorId, review.MovieId, review.Stars, token);

        // Assert
        result.ShouldBeTrue();

        var updatedReview = fixture.Context.Reviews.FirstOrDefault(a => a.Id == review.Id);

        updatedReview.Id.ShouldBe(review.Id);
        updatedReview.ReviewAuthorId.ShouldBe(review.AuthorId);
        updatedReview.ReviewedMovieId.ShouldBe(review.MovieId);
        updatedReview.Stars.ShouldBe(review.Stars);
        updatedReview.DateModified.ShouldBe(fixture.TimeProvider.GetUtcNow().UtcDateTime);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_False()
    {
        // Arrange
        var review = new UpdateReviewCommand
        {
            Id = Guid.Empty,
            AuthorId = fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "One").Id,
            MovieId = fixture.Context.Movies.FirstOrDefault(m => m.Title == "One").Id,
            Stars = 5
        };
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await fixture.Repository.UpdateReview(review.Id, review.AuthorId, review.MovieId, review.Stars, token);

        // Assert
        result.ShouldBeFalse();
    }

    #endregion Reviews
}

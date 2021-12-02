namespace CleanMinimalApi.Infrastructure.Tests.Integration.Persistance.InMemory.MovieReviews;

using CleanMinimalApi.Application.Reviews.Create;
using CleanMinimalApi.Application.Reviews.Update;
using Shouldly;
using Xunit;

[Collection("MovieReviews")]
public class MovieReviewsRepositoryTests
{
    private readonly MovieReviewsDataFixture fixture;

    public MovieReviewsRepositoryTests(MovieReviewsDataFixture fixture)
    {
        this.fixture = fixture;
    }

    #region Authors

    [Fact]
    public async void ReadAllAuthors_ShouldReturn_Authors()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.ReadAllAuthors(token);

        // Assert
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(3);
    }


    [Fact]
    public async void ReadAuthorById_ShouldReturn_Author()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;
        var author = this.fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "One");

        // Act
        var result = await repository.ReadAuthorById(author.Id, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.FirstName.ShouldBe("One");
        result.LastName.ShouldBe("One");
        result.Reviews.ShouldNotBeEmpty();
        result.Reviews.Count.ShouldBe(3);
    }

    [Fact]
    public async void ReadAuthorById_ShouldReturn_Null()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.ReadAuthorById(Guid.Empty, token);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async void AuthorExists_ShouldReturn_True()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;
        var author = this.fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "One");

        // Act
        var result = await repository.AuthorExists(author.Id, token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async void AuthorExists_ShouldReturn_False()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.AuthorExists(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    #endregion Authors

    #region Movies

    [Fact]
    public async void ReadAllMovies_ShouldReturn_Movies()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.ReadAllMovies(token);

        // Assert
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(3);
    }


    [Fact]
    public async void ReadMovieById_ShouldReturn_Movie()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;
        var movie = this.fixture.Context.Movies.FirstOrDefault(m => m.Title == "One");

        // Act
        var result = await repository.ReadMovieById(movie.Id, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Title.ShouldBe("One");
        result.Reviews.ShouldNotBeEmpty();
        result.Reviews.Count.ShouldBe(3);
    }

    [Fact]
    public async void ReadMovieById_ShouldReturn_Null()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.ReadMovieById(Guid.Empty, token);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async void MovieExists_ShouldReturn_True()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;
        var movie = this.fixture.Context.Movies.FirstOrDefault(m => m.Title == "One");

        // Act
        var result = await repository.MovieExists(movie.Id, token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async void MovieExists_ShouldReturn_False()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.MovieExists(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    #endregion Movies

    #region Reviews

    [Fact]
    public async void CreateReview_ShouldReturn_NewReviews()
    {
        // Arrange
        var review = new CreateCommand
        {
            AuthorId = this.fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "One").Id,
            MovieId = this.fixture.Context.Movies.FirstOrDefault(m => m.Title == "One").Id,
            Stars = 5
        };
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await this.fixture.Repository.CreateReview(review.AuthorId, review.MovieId, review.Stars, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.ReviewAuthorId.ShouldBe(review.AuthorId);
        result.ReviewedMovieId.ShouldBe(review.MovieId);
        result.Stars.ShouldBe(review.Stars);
        result.DateCreated.ShouldBe(this.fixture.DateTimeProvider.UtcNow);
        result.DateModified.ShouldBe(this.fixture.DateTimeProvider.UtcNow);

        // Cleanup
        _ = await this.fixture.Repository.DeleteReview(result.Id, token);
    }

    [Fact]
    public async void DeleteReview_ShouldReturn_True()
    {
        // Arrange
        var id = this.fixture.Context.Reviews.FirstOrDefault(r => r.Stars == 1).Id;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await this.fixture.Repository.DeleteReview(id, token);

        // Assert
        result.ShouldBeTrue();

        // Cleanup
        var authorId = this.fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "Three").Id;
        var movieId = this.fixture.Context.Movies.FirstOrDefault(m => m.Title == "Three").Id;
        _ = await this.fixture.Repository.CreateReview(authorId, movieId, 1, token);
    }

    [Fact]
    public async void DeleteReview_ShouldReturn_False()
    {
        // Arrange
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await this.fixture.Repository.DeleteReview(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async void ReadAllReviews_ShouldReturn_Reviews()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.ReadAllReviews(token);

        // Assert
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(9);
    }


    [Fact]
    public async void ReadReviewById_ShouldReturn_Review()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;
        var review = this.fixture.Context.Reviews.FirstOrDefault(m => m.Stars == 5);

        // Act
        var result = await repository.ReadReviewById(review.Id, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Stars.ShouldBe(5);
        result.ReviewAuthorId.ShouldNotBe(Guid.Empty);
        _ = result.ReviewAuthor.ShouldNotBeNull();
        result.ReviewAuthor.Id.ShouldBe(result.ReviewAuthorId);
        result.ReviewAuthor.FirstName.ShouldBe("Two");
        result.ReviewAuthor.LastName.ShouldBe("Two");
        result.ReviewedMovieId.ShouldNotBe(Guid.Empty);
        _ = result.ReviewedMovie.ShouldNotBeNull();
        result.ReviewedMovie.Id.ShouldBe(result.ReviewedMovieId);
        result.ReviewedMovie.Title.ShouldBe("Two");
    }

    [Fact]
    public async void ReadReviewById_ShouldReturn_Null()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.ReadReviewById(Guid.Empty, token);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async void ReviewExists_ShouldReturn_True()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;
        var review = this.fixture.Context.Reviews.FirstOrDefault(m => m.Stars == 5);

        // Act
        var result = await repository.ReviewExists(review.Id, token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async void ReviewExists_ShouldReturn_False()
    {
        // Arrange
        var repository = this.fixture.Repository;
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await repository.ReviewExists(Guid.Empty, token);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async void UpdateReview_ShouldReturn_True()
    {
        // Arrange
        var review = new UpdateCommand
        {
            Id = this.fixture.Context.Reviews.FirstOrDefault(a => a.Stars == 2).Id,
            AuthorId = this.fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "Two").Id,
            MovieId = this.fixture.Context.Movies.FirstOrDefault(m => m.Title == "Two").Id,
            Stars = 4
        };
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await this.fixture.Repository.UpdateReview(review.Id, review.AuthorId, review.MovieId, review.Stars, token);

        // Assert
        result.ShouldBeTrue();

        var updatedReview = this.fixture.Context.Reviews.FirstOrDefault(a => a.Id == review.Id);

        updatedReview.Id.ShouldBe(review.Id);
        updatedReview.ReviewAuthorId.ShouldBe(review.AuthorId);
        updatedReview.ReviewedMovieId.ShouldBe(review.MovieId);
        updatedReview.Stars.ShouldBe(review.Stars);
        updatedReview.DateModified.ShouldBe(this.fixture.DateTimeProvider.UtcNow);
    }

    [Fact]
    public async void UpdateReview_ShouldReturn_False()
    {
        // Arrange
        var review = new UpdateCommand
        {
            Id = Guid.Empty,
            AuthorId = this.fixture.Context.Authors.FirstOrDefault(a => a.FirstName == "One").Id,
            MovieId = this.fixture.Context.Movies.FirstOrDefault(m => m.Title == "One").Id,
            Stars = 5
        };
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await this.fixture.Repository.UpdateReview(review.Id, review.AuthorId, review.MovieId, review.Stars, token);

        // Assert
        result.ShouldBeFalse();
    }

    #endregion Reviews
}

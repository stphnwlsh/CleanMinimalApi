namespace CleanMinimalApi.Infrastructure.Tests.Integration.Databases.MovieReviews.Models;

using CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;
using Shouldly;
using Xunit;

[Collection("MovieReviews")]
public class ModelsTests()
{
    [Fact]
    public void Author_Model_Should_Have_Defaults()
    {
        // Act
        var author = new Author();

        // Assert
        author.Id.ShouldBe(Guid.Empty);
        author.FirstName.ShouldBeNull();
        author.LastName.ShouldBeNull();
        author.Reviews.ShouldBeNull();
        author.DateCreated.ShouldBe(DateTime.MinValue);
        author.DateModified.ShouldBe(DateTime.MinValue);
    }

    [Fact]
    public void Movie_Model_Should_Have_Defaults()
    {
        // Act
        var movie = new Movie();

        // Assert
        movie.Id.ShouldBe(Guid.Empty);
        movie.Title.ShouldBeNull();
        movie.Reviews.ShouldBeNull();
        movie.DateCreated.ShouldBe(DateTime.MinValue);
        movie.DateModified.ShouldBe(DateTime.MinValue);
    }

    [Fact]
    public void Review_Model_Should_Have_Defaults()
    {
        // Act
        var review = new Review();

        // Assert
        review.Id.ShouldBe(Guid.Empty);
        review.Stars.ShouldBe(default);
        review.ReviewAuthor.ShouldBeNull();
        review.ReviewedMovie.ShouldBeNull();
        review.DateCreated.ShouldBe(DateTime.MinValue);
        review.DateModified.ShouldBe(DateTime.MinValue);
    }
}

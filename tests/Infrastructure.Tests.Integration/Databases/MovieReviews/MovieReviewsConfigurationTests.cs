namespace CleanMinimalApi.Infrastructure.Tests.Integration.Databases.MovieReviews;

using Infrastructure.Databases.MoviesReviews.Models;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

[Collection("MovieReviews")]
public class MovieReviewsConfigurationTests(MovieReviewsDataFixture fixture)
{
    [Fact]
    public async Task Database_ShouldBe_Configured()
    {
        // Arrange
        var context = fixture.Context;
        var token = new CancellationTokenSource().Token;

        // Act
        var author = await context.Authors
            .Include(a => a.Reviews)
            .FirstOrDefaultAsync(a => a.FirstName == "One", token);
        var movie = await context.Movies
            .Include(m => m.Reviews)
            .FirstOrDefaultAsync(m => m.Title == "One", token);
        var review = await context.Reviews
            .Include(r => r.ReviewAuthor)
            .Include(r => r.ReviewedMovie)
            .FirstOrDefaultAsync(m => m.Stars == 5, token);

        // Assert
        context.Database.IsInMemory().ShouldBeTrue();

        _ = author.ShouldNotBeNull();
        _ = author.Id.ShouldBeOfType<Guid>();
        _ = author.FirstName.ShouldBeOfType<string>();
        _ = author.LastName.ShouldBeOfType<string>();
        _ = author.Reviews.ShouldBeOfType<HashSet<Review>>();

        _ = movie.ShouldNotBeNull();
        _ = movie.Id.ShouldBeOfType<Guid>();
        _ = movie.Title.ShouldBeOfType<string>();
        _ = movie.Reviews.ShouldBeOfType<HashSet<Review>>();

        _ = review.ShouldNotBeNull();
        _ = review.Id.ShouldBeOfType<Guid>();
        _ = review.Stars.ShouldBeOfType<int>();
        _ = review.ReviewAuthorId.ShouldBeOfType<Guid>();
        _ = review.ReviewAuthor.ShouldBeOfType<Author>();
        _ = review.ReviewedMovieId.ShouldBeOfType<Guid>();
        _ = review.ReviewedMovie.ShouldBeOfType<Movie>();
    }
}

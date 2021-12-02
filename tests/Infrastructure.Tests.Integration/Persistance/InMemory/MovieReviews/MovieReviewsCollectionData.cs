namespace CleanMinimalApi.Infrastructure.Tests.Integration.Persistance.InMemory.MovieReviews;

using System;
using CleanMinimalApi.Application.Entities;
using CleanMinimalApi.Infrastructure.Persistance.InMemory.MovieReviews;

internal static class MovieReviewsCollectionData
{
    public static MovieReviewsDbContext AddTestData(this MovieReviewsDbContext context)
    {
        var authors = new List<Author>
        {
            new Author { Id = Guid.NewGuid(), FirstName = "One", LastName = "One" },
            new Author { Id = Guid.NewGuid(), FirstName = "Two", LastName = "Two" },
            new Author { Id = Guid.NewGuid(), FirstName = "Three", LastName = "Three" }
        };

        context.Authors.AddRange(authors);

        var movies = new List<Movie>()
        {
            new Movie { Id = Guid.NewGuid(), Title = "One" },
            new Movie { Id = Guid.NewGuid(), Title = "Two" },
            new Movie { Id = Guid.NewGuid(), Title = "Three" }
        };

        context.Movies.AddRange(movies);

        var reviews = new List<Review>()
        {
            new Review { Id = Guid.NewGuid(), ReviewAuthorId = authors[0].Id, ReviewedMovieId = movies[0].Id, Stars = 1 },
            new Review { Id = Guid.NewGuid(), ReviewAuthorId = authors[0].Id, ReviewedMovieId = movies[1].Id, Stars = 2 },
            new Review { Id = Guid.NewGuid(), ReviewAuthorId = authors[0].Id, ReviewedMovieId = movies[2].Id, Stars = 3 },
            new Review { Id = Guid.NewGuid(), ReviewAuthorId = authors[1].Id, ReviewedMovieId = movies[0].Id, Stars = 4 },
            new Review { Id = Guid.NewGuid(), ReviewAuthorId = authors[1].Id, ReviewedMovieId = movies[1].Id, Stars = 5 },
            new Review { Id = Guid.NewGuid(), ReviewAuthorId = authors[1].Id, ReviewedMovieId = movies[2].Id, Stars = 4 },
            new Review { Id = Guid.NewGuid(), ReviewAuthorId = authors[2].Id, ReviewedMovieId = movies[0].Id, Stars = 3 },
            new Review { Id = Guid.NewGuid(), ReviewAuthorId = authors[2].Id, ReviewedMovieId = movies[1].Id, Stars = 2 },
            new Review { Id = Guid.NewGuid(), ReviewAuthorId = authors[2].Id, ReviewedMovieId = movies[2].Id, Stars = 1 }
        };

        context.Reviews.AddRange(reviews);

        _ = context.SaveChanges();

        return context;
    }
}

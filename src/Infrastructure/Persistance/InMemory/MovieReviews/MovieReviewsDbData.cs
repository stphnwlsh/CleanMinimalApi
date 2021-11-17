namespace CleanMinimalApi.Infrastructure.Persistance.InMemory.MovieReviews;

using System;
using Bogus;
using CleanMinimalApi.Domain.Authors.Entities;
using CleanMinimalApi.Domain.Movies.Entities;
using CleanMinimalApi.Domain.Reviews.Entities;

internal static class MovieReviewsDbData
{
    public static MovieReviewsDbContext AddData(this MovieReviewsDbContext context)
    {
        var authors = new Faker<Author>()
                   .RuleFor(a => a.Id, _ => Guid.NewGuid())
                   .RuleFor(a => a.FirstName, f => f.Person.FirstName)
                   .RuleFor(a => a.LastName, f => f.Person.LastName)
                   .Generate(15);

        context.AddRange(authors);

        var movies = new Faker<Movie>()
            .RuleFor(m => m.Id, _ => Guid.NewGuid())
            .RuleFor(m => m.Title, f => f.Company.CatchPhrase())
            .Generate(50);

        context.AddRange(movies);

        var reviews = new Faker<Review>()
            .RuleFor(r => r.Id, _ => Guid.NewGuid())
            .RuleFor(r => r.ReviewedMovieId, f => f.PickRandom(movies).Id)
            .RuleFor(r => r.ReviewAuthorId, f => f.PickRandom(authors).Id)
            .RuleFor(r => r.Stars, f => f.Random.Number(1, 5))
            .Generate(150);

        context.AddRange(reviews);

        _ = context.SaveChanges();

        return context;
    }
}

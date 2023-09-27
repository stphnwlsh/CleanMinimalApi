namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Extensions;

using System;
using Bogus;
using Models;

internal static class MovieReviewsDbContextExtensions
{
    public static MovieReviewsDbContext AddData(this MovieReviewsDbContext context)
    {
        var authors = new Faker<Author>()
            .RuleFor(a => a.Id, _ => Guid.NewGuid())
            .RuleFor(a => a.FirstName, f => f.Person.FirstName)
            .RuleFor(a => a.LastName, f => f.Person.LastName)
            .RuleFor(a => a.DateCreated, f => f.Date.Past())
            .RuleFor(a => a.DateModified, f => f.Date.Past())
            .Generate(15);

        context.AddRange(authors);

        var movies = new Faker<Movie>()
            .RuleFor(m => m.Id, _ => Guid.NewGuid())
            .RuleFor(m => m.Title, f => f.Company.CatchPhrase())
            .RuleFor(m => m.DateCreated, f => f.Date.Past())
            .RuleFor(m => m.DateModified, f => f.Date.Past())
            .Generate(50);

        context.AddRange(movies);

        var reviews = new Faker<Review>()
            .RuleFor(r => r.Id, _ => Guid.NewGuid())
            .RuleFor(r => r.ReviewedMovieId, f => f.PickRandom(movies).Id)
            .RuleFor(r => r.ReviewAuthorId, f => f.PickRandom(authors).Id)
            .RuleFor(r => r.Stars, f => f.Random.Number(1, 5))
            .RuleFor(r => r.DateCreated, f => f.Date.Past())
            .RuleFor(r => r.DateModified, f => f.Date.Past())
            .Generate(150);

        context.AddRange(reviews);

        _ = context.SaveChanges();

        return context;
    }
}

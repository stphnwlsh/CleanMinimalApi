namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Extensions;

using System;
using Bogus;
using Models;

internal static class MovieReviewsDbContextExtensions
{
    // A fixed seed and reference date give the same sample data on every run.
    private const int Seed = 20211014;
    private static readonly DateTime ReferenceDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static MovieReviewsDbContext AddData(this MovieReviewsDbContext context)
    {
        if (context.Authors.Any())
        {
            return context;
        }

        var authors = new Faker<Author>()
            .UseSeed(Seed)
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.FirstName, f => f.Person.FirstName)
            .RuleFor(a => a.LastName, f => f.Person.LastName)
            .RuleFor(a => a.DateCreated, f => f.Date.Past(refDate: ReferenceDate))
            .RuleFor(a => a.DateModified, f => f.Date.Past(refDate: ReferenceDate))
            .Generate(15);

        context.AddRange(authors);

        var movies = new Faker<Movie>()
            .UseSeed(Seed)
            .RuleFor(m => m.Id, f => f.Random.Guid())
            .RuleFor(m => m.Title, f => f.Company.CatchPhrase())
            .RuleFor(m => m.DateCreated, f => f.Date.Past(refDate: ReferenceDate))
            .RuleFor(m => m.DateModified, f => f.Date.Past(refDate: ReferenceDate))
            .Generate(50);

        context.AddRange(movies);

        var reviews = new Faker<Review>()
            .UseSeed(Seed)
            .RuleFor(r => r.Id, f => f.Random.Guid())
            .RuleFor(r => r.ReviewedMovieId, f => f.PickRandom(movies).Id)
            .RuleFor(r => r.ReviewAuthorId, f => f.PickRandom(authors).Id)
            .RuleFor(r => r.Stars, f => f.Random.Number(1, 5))
            .RuleFor(r => r.DateCreated, f => f.Date.Past(refDate: ReferenceDate))
            .RuleFor(r => r.DateModified, f => f.Date.Past(refDate: ReferenceDate))
            .Generate(150);

        context.AddRange(reviews);

        _ = context.SaveChanges();

        return context;
    }
}

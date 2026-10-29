namespace CleanMinimalApi.Application.Reviews.Entities;

using Application.Authors.Entities;
using Application.Movies.Entities;

/// <summary>
/// A star rating an author has given a movie.
/// </summary>
/// <param name="Id">The unique identifier of the review.</param>
/// <param name="Stars">The star rating, from 1 to 5.</param>
/// <param name="ReviewedMovie">The movie that was reviewed.</param>
/// <param name="ReviewAuthor">The author who wrote the review.</param>
public record Review(
    Guid Id,
    int Stars,
    ReviewedMovie? ReviewedMovie = null,
    ReviewAuthor? ReviewAuthor = null);

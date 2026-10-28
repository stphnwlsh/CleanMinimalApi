namespace CleanMinimalApi.Application.Movies.Entities;

/// <summary>
/// The movie a review is about.
/// </summary>
/// <param name="Id">The unique identifier of the movie.</param>
/// <param name="Title">The title of the movie.</param>
public record ReviewedMovie(Guid Id, string Title);

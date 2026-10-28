namespace CleanMinimalApi.Application.Movies.Entities;

using Application.Reviews.Entities;

/// <summary>
/// A movie that can be reviewed.
/// </summary>
/// <param name="Id">The unique identifier of the movie.</param>
/// <param name="Title">The title of the movie.</param>
/// <param name="Reviews">The reviews written about the movie.</param>
public record Movie(Guid Id, string Title, ICollection<Review>? Reviews = null);
